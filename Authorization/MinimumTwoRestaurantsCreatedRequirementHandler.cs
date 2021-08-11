using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Restaurant_API.Entities;

namespace Restaurant_API.Authorization
{
    public class MinimumTwoRestaurantsCreatedRequirementHandler : AuthorizationHandler<MinimumTwoRestaurantsCreatedRequirement>
    {
        private readonly RestaurantDbContext _context;

        public MinimumTwoRestaurantsCreatedRequirementHandler(RestaurantDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumTwoRestaurantsCreatedRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdRestaurantCount = _context.Restaurants.Count(x => x.CreatedById == userId);

            if(createdRestaurantCount >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}