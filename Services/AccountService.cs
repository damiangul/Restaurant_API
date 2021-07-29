using Restaurant_API.Entities;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly RestaurantDbContext _context;

        public AccountService(RestaurantDbContext context)
        {
            _context = context;
        }
        public void RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                Nationality = dto.Nationality,
                RoleId = dto.RoleId
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}