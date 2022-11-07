using RepositoryLayer.Repository;

namespace RepositoryLayer.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IOrderRepository OrderRepository { get; set; }
        public IMealRepository MealRepository { get; set; }
        public IMealInOrderRepository MealInOrderRepository { get; set; }
        public IIngredientRepository IngredientRepository { get; set; }
        public IPricelistRepository PricelistRepository { get; set; }
        public void SaveChanges();
    }
}
