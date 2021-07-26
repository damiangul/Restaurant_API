using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant_API.Entities;

namespace Restaurant_API
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext.Restaurants.Include(r => r.Address).Include(r => r.Dishes).FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                return null;

            var result = _mapper.Map<RestaurantDto>(restaurant);

            return result;
        }

        public IEnumerable<RestaurantDto> GetAll()
        {
            var restaurants = _dbContext.Restaurants.Include(r => r.Address).Include(r => r.Dishes).ToList();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

            return restaurantsDtos;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public bool Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                return false;

            _dbContext.Remove(restaurant);
            _dbContext.SaveChanges();

            return true;
        }

        public bool UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if(restaurant is null)
                return false;

            restaurant.Name = restaurantUpdateDto.Name;
            restaurant.Description = restaurantUpdateDto.Description;
            restaurant.HasDelivery = restaurantUpdateDto.HasDelivery;

            _dbContext.SaveChanges();

            return true;
        }
    }
}