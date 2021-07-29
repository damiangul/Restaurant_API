using Microsoft.AspNetCore.Identity;
using Restaurant_API.Entities;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(RestaurantDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            

            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            var hashedPaswword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPaswword;

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}