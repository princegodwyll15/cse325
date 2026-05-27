using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Data;

namespace BlazingPizza.Controllers;

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
        .Include(o => o.Pizzas!).ThenInclude(p => p.Special!)
        .Include(o => o.Pizzas).ThenInclude(p => p.Toppings!).ThenInclude(t => t.Topping!)
        .OrderByDescending(o => o.CreatedTime)
        .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

[HttpPost]
public async Task<ActionResult<int>> PlaceOrder(Order order)
{
    order.CreatedTime = DateTime.UtcNow;

    foreach (var pizza in order.Pizzas)
    {
        pizza.Order = order;
    }

    _db.Orders.Add(order);

    await _db.SaveChangesAsync();

    return Ok(order.OrderId);
}

    [HttpGet("{orderId}")]
    public async Task<ActionResult<Order>> GetOrder(int orderId)
    {
        if(orderId <= 0)
        {
            return BadRequest("Invalid order ID.");
        }

        return await _db.Orders
            .Include(o => o.Pizzas!).ThenInclude(p => p.Special!)
            .Include(o => o.Pizzas!).ThenInclude(p => p.Toppings!).ThenInclude(t => t.Topping!)
            .FirstAsync(o => o.OrderId == orderId);
    }

    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null)
        {
            return NotFound();
        }

        _db.Orders.Remove(order);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}