using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer;
using RepositoryLayer.Repository;
using RepositoryLayer.UnitOfWork;

namespace ServiceLayer.Extensions
{
    public static class DependenciesInjection
    {
        public static void AddRestaurantDbContext(this IServiceCollection services, string connectionStr)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionStr);
            });
        }
        public static void AddRepositoryDependencies(this IServiceCollection services)
        {
            services.AddScoped<DbContext, ApplicationDbContext>();

            services.AddScoped<IIngredientRepository, IngredientRepository>();
            
            services.AddScoped<IMealRepository, MealRepository>();
            
            services.AddScoped<IOrderRepository, OrderRepository>();
            
            services.AddScoped<IMealInOrderRepository, MealInOrderRepository>();
            
            services.AddScoped<IPricelistRepository, PricelistRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
