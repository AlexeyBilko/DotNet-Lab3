using AutoBogus;
using FluentAssertions;
using System;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RepositoryLayer;
using RepositoryLayer.Repository;

namespace RepositoryLayer.Tests
{
    public class RepositoryTests
    {
        public class OrderRepositoryTests
        {
            [Test]
            public void GetOrdersByTable_CreateOrder_ListContainingOneElement()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var tableNumber = AutoFaker.Generate<int>();
                var order = new Order()
                {
                    OrderedTime = DateTime.Now,
                    TableNumber = tableNumber
                };

                using (var context = new ApplicationDbContext(options))
                {
                    context.Orders.Add(order);
                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);
                    List<Order> orders = target.GetOrdersByTable(tableNumber).ToList();

                    orders.First().Should().BeEquivalentTo(order);

                    context.Database.EnsureDeleted();
                }
            }

            [Test]
            public void GetAll_CreateOrders_CountEquals()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                using (var context = new ApplicationDbContext(options))
                {
                    context.Orders.Add(AutoFaker.Generate<Order>());
                    context.Orders.Add(AutoFaker.Generate<Order>());
                    context.Orders.Add(AutoFaker.Generate<Order>());
                    context.SaveChanges();
                }

                // Use a clean instance of the context to run the test
                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);
                    List<Order> orders = target.GetAllAsync().Result.ToList();

                    orders.Count.Should().Be(3);

                    context.Database.EnsureDeleted();
                }

                //var userDbSet = new TestDbSet<Order>();
                //userDbSet.Add(new Order());
                //userDbSet.Add(new Order());

                //var contextMock = new Mock<ApplicationDbContext>();
                //contextMock.Setup(dbContext => dbContext.Orders).Returns(userDbSet);
                //// arrange
                //var order = AutoFaker.Generate<Order>();

                //context
                //    .OrdersSubstitute
                //    .Add(order)
                //    .Returns(order);

                //// aciton
                //target.Create(order);

                //// assert
                //context.OrdersSubstitute.Handler.Received().Add(Arg.Any<Order>());
            }

            [Test]
            public void Create_Order_ElementAdded()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var order = AutoFaker.Generate<Order>();

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);
                    target.CreateAsync(order);
                }

                using (var context = new ApplicationDbContext(options))
                {
                    List<Order> orders = context.Orders.ToList();

                    orders.Should().ContainEquivalentOf(order);

                    context.Database.EnsureDeleted();
                }
            }

            [Test]
            public void Delete_Order_ElementRemoved()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var order = AutoFaker.Generate<Order>();

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);
                    target.CreateAsync(order);
                    target.DeleteAsync(order);
                }

                using (var context = new ApplicationDbContext(options))
                {
                    List<Order> orders = context.Orders.ToList();

                    orders.Should().NotContainEquivalentOf(order);

                    context.Database.EnsureDeleted();
                }
            }

            [Test]
            public void Update_Order_ElementUpdated()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var order = AutoFaker.Generate<Order>();

                var updatedOrder = order;

                var updatedTableNumber = AutoFaker.Generate<int>();
                updatedOrder.TableNumber = updatedTableNumber;

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);
                    target.CreateAsync(order);
                    target.UpdateAsync(updatedOrder);
                }

                using (var context = new ApplicationDbContext(options))
                {
                    List<Order> orders = context.Orders.ToList();

                    orders.First().Should().BeEquivalentTo(updatedOrder);

                    context.Database.EnsureDeleted();
                }
            }

            //[Test]
            //public void Get_Order_GetsElement()
            //{
            //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //        .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
            //        .Options;

            //    var order = new Order()
            //    {
            //        Id = 1,
            //        TableNumber = AutoFaker.Generate<int>(),
            //        OrderedTime = DateTime.Now
            //    };

            //    using (var context = new ApplicationDbContext(options))
            //    {
            //        OrderRepository target = new OrderRepository(context);

            //        var tmp_ = target.CreateAsync(order).Result;

            //        target.Get(order.Id).Should().BeEquivalentTo(order);

            //        context.Database.EnsureDeleted();
            //    }
            //}


            [Test]
            public void SaveChanges_AddedOrder_ElementExists()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var order = AutoFaker.Generate<Order>();

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);

                    target.CreateAsync(order);
                    target.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    List<Order> orders = context.Orders.ToList();

                    orders.First().Should().BeEquivalentTo(order);

                    context.Database.EnsureDeleted();
                }
            }


            [Test]
            public void SaveChangesAsync_AddedOrder_ElementExists()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var order = AutoFaker.Generate<Order>();

                using (var context = new ApplicationDbContext(options))
                {
                    OrderRepository target = new OrderRepository(context);

                    target.CreateAsync(order);
                    target.SaveChangesAsync();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    List<Order> orders = context.Orders.ToList();

                    orders.First().Should().BeEquivalentTo(order);

                    context.Database.EnsureDeleted();
                }
            }
        }

        public class MealRepositoryTests
        {
            [Test]
            public void GetMealByName_CreateMeal_ListContainingOneElement()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var mealName = AutoFaker.Generate<string>();
                var meal = new Meal()
                {
                    Name = mealName,
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                using (var context = new ApplicationDbContext(options))
                {
                    context.Meals.Add(meal);
                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    MealRepository target = new MealRepository(context);
                    List<Meal> meals = target.GetMealsByName(mealName).ToList();

                    meals.First().Should().BeEquivalentTo(meal);

                    context.Database.EnsureDeleted();
                }
            }

            [Test]
            public void GetMealsInOrder_CreateMealInOrder_ListContainingOneElement()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var meal = AutoFaker.Generate<Meal>();
                var order = AutoFaker.Generate<Order>();

                var mealInOrder = new MealInOrder
                {
                    Meal = meal,
                    Order = order
                };

                using (var context = new ApplicationDbContext(options))
                {
                    context.Meals.Add(meal);
                    context.Orders.Add(order);
                    context.MealInOrders.Add(mealInOrder);
                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    MealRepository target = new MealRepository(context);
                    List<Meal> meals = target.GetMealsInOrder(order.Id).ToList();

                    meals.First().Should().BeEquivalentTo(meal);

                    context.Database.EnsureDeleted();
                }
            }
        }

        public class MealInOrderRepositoryTests
        {
            [Test]
            public void RemoveMealFromOrder_Meal_DoesnotContainElement()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var meal = AutoFaker.Generate<Meal>();
                var order = AutoFaker.Generate<Order>();

                var mealInOrder = new MealInOrder
                {
                    Meal = meal,
                    Order = order
                };

                using (var context = new ApplicationDbContext(options))
                {
                    context.Meals.Add(meal);
                    context.Orders.Add(order);
                    context.MealInOrders.Add(mealInOrder);
                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    MealInOrderRepository target = new MealInOrderRepository(context);
                    target.FindMealAndRemoveFromOrder(meal, order.Id);

                    context.MealInOrders.Should().NotContainEquivalentOf(mealInOrder);

                    context.Database.EnsureDeleted();
                }
            }
        }

        public class IngredientRepositoryTests
        {
            //[Test]
            //public void GetIngredientInMeal_Meal_GetsAllIngredients()
            //{
            //    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //        .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
            //        .Options;

            //    var meal = AutoFaker.Generate<Meal>();
            //    var ingredient1 = new Ingredient()
            //    {
            //        MealId = meal.Id,
            //        Name = AutoFaker.Generate<string>(),
            //        Weight = AutoFaker.Generate<float>()
            //    };
            //    var ingredient2 = new Ingredient()
            //    {
            //        MealId = meal.Id,
            //        Name = AutoFaker.Generate<string>(),
            //        Weight = AutoFaker.Generate<float>()
            //    };
            //    var ingredient3 = new Ingredient()
            //    {
            //        MealId = meal.Id,
            //        Name = AutoFaker.Generate<string>(),
            //        Weight = AutoFaker.Generate<float>()
            //    };


            //    using (var context = new ApplicationDbContext(options))
            //    {
            //        context.Meals.Add(meal);
            //        context.Ingredients.Add(ingredient1);
            //        context.Ingredients.Add(ingredient2);
            //        context.Ingredients.Add(ingredient3);
            //        context.SaveChanges();
            //    }

            //    using (var context = new ApplicationDbContext(options))
            //    {
            //        IngredientRepository target = new IngredientRepository(context);
            //        List<Ingredient> ingredients = target.GetIngredientInMeal(meal.Id).ToList();

            //        var expectedResult = new[] { target.Get(ingredient1.Id), target.Get(ingredient2.Id), target.Get(ingredient3.Id)};

            //        ingredients.Should().BeEquivalentTo(expectedResult);

            //        context.Database.EnsureDeleted();
            //    }
            //}
        }

        public class PricelistRepositoryTests
        {
            [Test]
            public void GetPriceByMeal_Meal_GetsCorrectPrice()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                    .Options;

                var meal = AutoFaker.Generate<Meal>();
                var price = AutoFaker.Generate<float>();

                using (var context = new ApplicationDbContext(options))
                {
                    context.Meals.Add(meal);
                    context.Pricelist.Add(new Pricelist()
                    {
                        MealId = meal.Id,
                        Price = price
                    });
                    context.SaveChanges();
                }

                using (var context = new ApplicationDbContext(options))
                {
                    PricelistRepository target = new PricelistRepository(context);
                    Pricelist pricelist = target.GetPriceByMeal(meal.Id);

                    pricelist.Price.Should().Be(price);

                    context.Database.EnsureDeleted();
                }
            }
        }
    }
}
