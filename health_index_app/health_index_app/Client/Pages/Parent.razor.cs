﻿using health_index_app.Client.Services;
using health_index_app.Shared.DTObjects;
using health_index_app.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace health_index_app.Client.Pages
{
    public partial class Parent
    {
        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject]
        protected IFoodAPIServices FoodAPIServices { get; set; }
        [Inject]
        protected IMealFoodAPIServices MealFoodAPIServices { get; set; }
        [Inject]
        protected IMealsAPIServices MealAPIServices { get; set; }
        [Inject]
        protected IUserMealsAPIServices UserMealsAPIServices { get; set; }
        protected ParentAPIServices parentAPIServices { get; set; } = new( new HttpClient());


        List<string> childUsernames = null!;
        List<MealDTO> childMeals = null!;
        Dictionary<int, List<Food>> childMealFoods = null!;


        protected override async Task OnInitializedAsync()
        {
            var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
            if (UserAuth is not null && UserAuth.IsAuthenticated)
            {
                childUsernames = await parentAPIServices.GetChildUsernames();
                childMeals = await parentAPIServices.GetChildMeals();
                //foreach(var meal in childMeals)
                //{
                //    childMealFoods.Add(meal.MealId, await parentAPIServices.GetChildFoods(meal.MealId));
                //}
            }
        }

        private async void GetChildUsernames()
        {
            childUsernames = await parentAPIServices.GetChildUsernames();
        }

    }
}
