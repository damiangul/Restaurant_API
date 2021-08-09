using System.Collections.Generic;
using System.Security.Claims;
using Restaurant_API.Entities;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto, int userId);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);

        void Delete(int id, ClaimsPrincipal user);
        void UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto, ClaimsPrincipal user);
    }
}