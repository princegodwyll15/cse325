public class Topping
{
    public int ToppingId { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string GetFormattedPrice() => $"₵{Price:0.00}";
}