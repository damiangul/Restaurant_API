using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
    }
}