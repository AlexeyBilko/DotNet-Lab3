using RepositoryLayer.Repository;

namespace RepositoryLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext context;
        public IIngredientRepository IngredientRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IMealRepository MealRepository { get; set; }
        public IMealInOrderRepository MealInOrderRepository { get; set; }
        public IPricelistRepository PricelistRepository { get; set; }

        public UnitOfWork(ApplicationDbContext context,
            IIngredientRepository ingredientRepository,
            IOrderRepository orderRepository,
            IMealRepository mealRepository,
            IMealInOrderRepository mealInOrderRepository,
            IPricelistRepository pricelistRepository)
        {
            this.context = context;
            IngredientRepository = ingredientRepository;
            OrderRepository = orderRepository;
            MealRepository = mealRepository;
            MealInOrderRepository = mealInOrderRepository;
            PricelistRepository = pricelistRepository;
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        ~UnitOfWork()
        {
            context.Dispose();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
