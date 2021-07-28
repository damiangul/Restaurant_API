using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
    }
}