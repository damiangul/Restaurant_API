using Microsoft.AspNetCore.Authorization;

namespace Restaurant_API.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }
        public MinimumAgeRequirement(int minimumAge) 
        {
            MinimumAge = minimumAge;
        }
    }
}