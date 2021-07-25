using System.Collections.Generic;

namespace Restaurant_API
{
    public interface IRestaurantService
    {
        int Create(CreateRestaurantDto dto);
        IEnumerable<RestaurantDto> GetAll();
        RestaurantDto GetById(int id);

        bool Delete(int id);
    }
}