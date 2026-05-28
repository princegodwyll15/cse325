using System.Collections.Generic;
using System.Linq;

namespace BlazingPizza.Data;

public class Order
{
    public int OrderId { get; set; }
     public string UserId { get; set; } = string.Empty;
    public List<Pizza> Pizzas { get; set; } = new();
    public DateTime CreatedTime { get; set; } = DateTime.Now;
     public Address DeliveryAddress { get; set; } = new Address();
    public decimal GetTotalPrice() => Pizzas.Sum(p => p.GetTotalPrice());
    public string GetFormattedTotalPrice() => $"${GetTotalPrice():0.00}";
}