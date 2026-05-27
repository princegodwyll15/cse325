using System.Text.Json.Serialization;
using BlazingPizza.Data;
public class PizzaTopping
{
    public int PizzaToppingId { get; set; }

    public int PizzaId { get; set; }

    [JsonIgnore]
    public Pizza? Pizza { get; set; } = default!;

    public int ToppingId { get; set; }
    
    [JsonIgnore]
    public Topping? Topping { get; set; } = default!;
}

