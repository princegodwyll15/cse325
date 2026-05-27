using System.Text.Json.Serialization;
namespace BlazingPizza.Data;

public class Pizza
{
    public const int DefaultSize = 12;
    public const int MinimumSize = 9;
    public const int MaximumSize = 17;

    public int PizzaId { get; set; }

    public int OrderId { get; set; }    
    
    [JsonIgnore]
    public Order? Order { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool Vegetarian { get; set; }

    public bool Vegan { get; set; }

    public int Size { get; set; } = DefaultSize;

    public int SpecialId { get; set; }

    public PizzaSpecial? Special { get; set; } = default!;

    public List<PizzaTopping>? Toppings { get; set; } = new();

    public decimal GetBasePrice()
    {
        return Special != null
            ? Special.BasePrice * Size / DefaultSize
            : Price;
    }

    public decimal GetTotalPrice()
    {
        var total = GetBasePrice();

        if (Toppings != null && Toppings.Any())
            total += Toppings.Sum(t => t.Topping?.Price ?? 0);

        return total;
    }

    public string GetFormattedTotalPrice() =>
        $"₵{GetTotalPrice():0.00}";
}