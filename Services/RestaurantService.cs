using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Restaurant_API.Authorization;
using Restaurant_API.Entities;
using Restaurant_API.Exceptions;
using Restaurant_API.Models;

namespace Restaurant_API.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext.Restaurants.Include(r => r.Address).Include(r => r.Dishes).FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

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
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResorceOperationRequirement(ResourceOperation.Delete)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public void UpdateRestaurant(int id, RestaurantUpdateDto restaurantUpdateDto)
        {
            var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);

            if(restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant, new ResorceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            restaurant.Name = restaurantUpdateDto.Name;
            restaurant.Description = restaurantUpdateDto.Description;
            restaurant.HasDelivery = restaurantUpdateDto.HasDelivery;

            _dbContext.SaveChanges();
        }
    }
}