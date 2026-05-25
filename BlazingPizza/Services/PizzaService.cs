using BlazingPizza.Data;

namespace BlazingPizza.Services
{
    public class PizzaService
    {
        public Task<List<Pizza>> GetPizzaAsync()
        {
            // call data access here
            return Task.FromResult(new List<Pizza>
            {
                new Pizza
                {
                    PizzaId = 1,
                    Name = "Margherita",
                    Description = "Classic pizza with tomato sauce, mozzarella cheese, and fresh basil.",
                    Price = 8.99m,
                    Vegetarian = true,
                    Vegan = false
                },
                new Pizza
                {
                    PizzaId = 2,
                    Name = "Pepperoni",
                    Description = "Spicy pepperoni slices on a bed of tomato sauce and mozzarella cheese.",
                    Price = 9.99m,
                    Vegetarian = false,
                    Vegan = false
                },
                new Pizza
                {
                    PizzaId = 3,
                    Name = "Veggie Delight",
                    Description = "Loaded with bell peppers, onions, mushrooms, olives, and tomato sauce.",
                    Price = 10.99m,
                    Vegetarian = true,
                    Vegan = true
                },
                    new Pizza
                    {
                        PizzaId = 4,
                        Name = "BBQ Chicken",
                        Description = "Grilled chicken, BBQ sauce, red onions, and mozzarella cheese.",
                        Price = 11.99m,
                        Vegetarian = false,
                        Vegan = false
                    },
                    new Pizza
                    {
                        PizzaId = 5,
                        Name = "Hawaiian",
                        Description = "Ham, pineapple, tomato sauce, and mozzarella cheese.",
                        Price = 9.49m,
                        Vegetarian = false,
                        Vegan = false
                    }

            });
        }
    }
}