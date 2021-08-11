using Microsoft.AspNetCore.Authorization;

namespace Restaurant_API.Authorization
{
    public class MinimumTwoRestaurantsCreatedRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantsCreated { get; set; }

        public MinimumTwoRestaurantsCreatedRequirement(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }
    }
}