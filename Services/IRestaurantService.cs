using System.Collections.Generic;
using Restaurant_API.Entities;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);

        void Delete(int id);
        void UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto);
    }
}