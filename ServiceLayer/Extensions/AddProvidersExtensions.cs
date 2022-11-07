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
        public static void AddDbContext(this IServiceCollection services, string connectionStr)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionStr);
            });
        }

        public static void AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<IService<Ingredient, IngredientDTO>, PlanService>();
            services.AddScoped<PlanService, PlanService>();
            services.AddScoped<IRepository<Ingredient>, IngredientRepository>();

            services.AddScoped<IService<Meal,MealDTO>, SubPagesService>();
            services.AddScoped<SubPagesService, SubPagesService>();
            services.AddScoped<IRepository<Meal>, MealRepository>();

            services.AddScoped<IService<Order, OrderDTO>, SubPagesStatService>();
            services.AddScoped<SubPagesStatService, SubPagesStatService>();
            services.AddScoped<IRepository<Order>, OrderRepository>();

            services.AddScoped<IService<MealInOrder, MealInOrderDTO>, TransactionService>();
            services.AddScoped<TransactionService, TransactionService>();
            services.AddScoped<IRepository<MealInOrder>, MealInOrderRepository>();

            services.AddScoped<IService<Pricelist, PricelistDTO>, UserPlanService>();
            services.AddScoped<UserPlanService, UserPlanService>();
            services.AddScoped<IRepository<Pricelist>, PricelistRepository>();

            services.AddTransient<DbContext, LeadSubContext>();
        }
    }
}
