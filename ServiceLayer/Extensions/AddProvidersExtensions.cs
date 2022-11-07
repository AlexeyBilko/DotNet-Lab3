using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer;
using RepositoryLayer.Repository;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace ServiceLayer.Extensions
{
    public static class AddProvidersExtensions
    {
        public static void AddServicesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IngredientService, IngredientService>();

            services.AddScoped<IMealService, MealService>();
            services.AddScoped<MealService, MealService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<OrderService, OrderService>();

            services.AddScoped<IMealService, MealService>();
            services.AddScoped<MealService, MealService>();

            services.AddScoped<IPricelistService, PricelistService>();
            services.AddScoped<PricelistService, PricelistService>();
        }
    }
}
