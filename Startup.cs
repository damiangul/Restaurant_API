using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Restaurant_API.Authorization;
using Restaurant_API.Entities;
using Restaurant_API.Middleware;
using Restaurant_API.Models;
using Restaurant_API.Models.Validators;
using Restaurant_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationSettings = new AuthenticationSettings();
            Configuration.GetSection("Authentication").Bind(authenticationSettings);
            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg => {
                //Nie wymuszamy logowania tylko przez protokół HTTPS
                cfg.RequireHttpsMetadata = false;
                //Czy token powinien zostać zapisany po stronie serwera do celów autentykacji
                cfg.SaveToken = true;
                //Parametry walidacji. Sprawdzenie czy token wysłany przez klienta jest zgodny z tym co wie serwer.
                cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    //Wydawca danego tokenu.
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    //Jakie podmioty mogą używać tego tokenu. Jest to samo, ponieważ sami generujemy dane tokeny i tylko my będziemy mogli ich używać.
                    ValidAudience = authenticationSettings.JwtIssuer,
                    //Klucz prywatny.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            //Customowa polityka. Dostep maja tylko uzytkownicy z polski i niemiec. [Authorize(Policy = "HasNationality")]
            services.AddAuthorization(options => {
                options.AddPolicy("HasNationality", builder => builder.RequireClaim("Nationality", "German", "Polish"));
                options.AddPolicy("Atleast20", builder => builder.AddRequirements(new MinimumAgeRequirement(20)));
                options.AddPolicy("CreatedAtLeastTwoRestaurants", builder => builder.AddRequirements(new MinimumTwoRestaurantsCreatedRequirement(2)));
            });

            services.AddScoped<IAuthorizationHandler, MinimumAgeRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, ResorceOperationRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, MinimumTwoRestaurantsCreatedRequirementHandler>();

            services.AddControllers().AddFluentValidation();
            services.AddDbContext<RestaurantDbContext>();
            services.AddScoped<RestaurantSeeder>();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IDishService, DishService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ErrorHandlingMiddleware>();
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
            services.AddScoped<IValidator<RestaurantQuery>, RestaurantQueryValidator>();

            services.AddScoped<RequestTimeMiddleware>();

            services.AddScoped<IUserContextService, UserContextService>();
            services.AddHttpContextAccessor();

            services.AddSwaggerGen();

            services.AddCors(options => {
                options.AddPolicy("FrontEndClient", builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins(Configuration["AllowedOrigins"]));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestaurantSeeder restaurantSeeder)
        {
            app.UseCors("FrontEndClient");

            restaurantSeeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMiddleware<RequestTimeMiddleware>();

            

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestaurantAPI");
            });

            app.UseRouting();
            app.UseAuthentication();
            //Wazne jest aby tutaj wywołać. Miedzy tymi dwoma middleware autoryzujemy uzytkowników.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
