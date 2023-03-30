﻿using health_index_app.Client.Pages;
using health_index_app.Server.Data;
using health_index_app.Server.Models;
using health_index_app.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;

namespace health_index_app.Server.Controllers
{
    [ApiController]
    [Route("usermeals")]
    public class UserMealsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger<FatSecretController> _logger;

        public UserMealsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IConfiguration config, ILogger<FatSecretController> logger)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
            _logger = logger;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<UserMealDTO>> CreateUserMeal([FromBody] UserMealDTO userMealDTO)
        {
            UserMeal userMeal = new UserMeal()
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                MealId = userMealDTO.MealId,
                Name = userMealDTO.Name,
            };

            _context.UserMeals.Add(userMeal);
            await _context.SaveChangesAsync();
            _context.Entry(userMeal).Reload();

            userMealDTO.Id = userMeal.Id;

            return Ok(userMealDTO);
        }

        [HttpGet]
        [Route("read")]
        public async Task<ActionResult<UserMealDTO>> ReadUserMeal(int mealId)
        {
            UserMeal userMeal = await _context.UserMeals.Where(m => m.MealId == mealId).FirstOrDefaultAsync();

            if (userMeal == null)
                return NotFound();

            UserMealDTO userMealDTO = new UserMealDTO
            {
                Id = userMeal.Id,
                Name = userMeal.Name,
                MealId = userMeal.MealId
            };

            return Ok(userMealDTO);
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<bool>> UpdateUserMeal([FromBody] UserMealDTO userMealDTO)
        {
            UserMeal userMeal = await _context.UserMeals
                .Where(m => m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && m.MealId == userMealDTO.MealId)
                .FirstOrDefaultAsync();

            if (userMeal == null) 
                return NotFound(false);

            userMeal.MealId = userMealDTO.MealId;
            userMeal.Name = userMealDTO.Name;

            _context.UserMeals.Update(userMeal);
            await _context.SaveChangesAsync();


            return Ok(true);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<bool>> DeleteUserMeal([FromBody] int mealId)
        {
            //return GetDummyCurrentWeather();
            UserMeal userMeal = await _context.UserMeals
                .Where(m => m.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier) && m.MealId == mealId)
                .FirstOrDefaultAsync();

            if (userMeal == null)
            {
                return NotFound(false);
            }

            _context.UserMeals.Remove(userMeal);
            await _context.SaveChangesAsync();

            return Ok(true);
        }

        [HttpGet]
        [Authorize]
        [Route("get-all-meal-ids")]
        public async Task<ActionResult<List<int>>> GetAllUserMealIds()
        {
            List<int> mealIds = await _context.UserMeals
                .Where(um => um.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(um => um.MealId)
                .ToListAsync();
                
            return Ok(mealIds);
        }
    }
}
