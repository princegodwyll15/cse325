using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizza.Data;

[Route("orders")]
[ApiController]
public class OrdersController : Controller
{
    private readonly PizzaStoreContext _db;

    public OrdersController(PizzaStoreContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderWithStatus>>> GetOrders()
    {
        var orders = await _db.Orders
        .Include(o => o.Pizzas).ThenInclude(p => p.Special)
        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings).ThenInclude(t => t.Topping)
        .OrderByDescending(o => o.CreatedTime)
        .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<int>> PlaceOrder(Order order)
    {
        order.CreatedTime = DateTime.UtcNow;

        // Attach existing Special and Topping records instead of inserting duplicates
        foreach (var pizza in order.Pizzas)
        {
            _db.Entry(pizza.Special).State = EntityState.Unchanged;

            foreach (var topping in pizza.Toppings)
            {
                _db.Entry(topping.Topping).State = EntityState.Unchanged;
            }
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return Ok(order.OrderId);
    }
}