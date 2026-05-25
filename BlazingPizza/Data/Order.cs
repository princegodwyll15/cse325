using System.Collections.Generic;
using System.Linq;

namespace BlazingPizza.Data;

public class Order
{
    public int OrderId { get; set; }
    public List<Pizza> Pizzas { get; set; } = new();
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public decimal GetTotalPrice() => Pizzas.Sum(p => p.GetTotalPrice());
    public string GetFormattedTotalPrice() => $"₵{GetTotalPrice():0.00}";
}