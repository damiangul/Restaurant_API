using System.Collections.Generic;
using System.Security.Claims;
using Restaurant_API.Entities;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll(string searchPhrase);
        RestaurantDto GetById(int id);

        void Delete(int id);
        void UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto);
    }
}