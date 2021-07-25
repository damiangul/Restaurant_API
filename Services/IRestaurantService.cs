using System.Collections.Generic;
using Restaurant_API.Entities;

namespace Restaurant_API
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);

        bool Delete(int id);
        bool UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto);
    }
}