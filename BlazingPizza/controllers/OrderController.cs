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
        .AsSplitQuery()
        .OrderByDescending(o => o.CreatedTime)
        .ToListAsync();

        return orders.Select(o => OrderWithStatus.FromOrder(o)).ToList();
    }

[HttpPost]
public async Task<ActionResult<int>> PlaceOrder(PlaceOrderDto orderDto)
{
    var order = new Order();

    // Map minimal DTO to Order and Pizza entities
    foreach (var p in orderDto.Pizzas)
    {
        order.Pizzas.Add(new Pizza
        {
            SpecialId = p.SpecialId,
            Size = p.Size
        });
    }

    order.CreatedTime = DateTime.UtcNow;

    var specialIds = order.Pizzas
        .Where(p => p.SpecialId > 0)
        .Select(p => p.SpecialId)
        .Distinct()
        .ToList();

    var specials = await _db.Specials
        .Where(s => specialIds.Contains(s.Id))
        .ToDictionaryAsync(s => s.Id);

    foreach (var pizza in order.Pizzas)
    {
        pizza.Order = order;

        if (specials.TryGetValue(pizza.SpecialId, out var special))
        {
            pizza.Special = special;

            if (string.IsNullOrWhiteSpace(pizza.Name))
            {
                pizza.Name = special.Name;
            }

            if (string.IsNullOrWhiteSpace(pizza.Description))
            {
                pizza.Description = special.Description;
            }

            if (pizza.Price <= 0)
            {
                pizza.Price = special.BasePrice;
            }
        }
    }

    _db.Orders.Add(order);

    await _db.SaveChangesAsync();

    return Ok(order.OrderId);
}

[HttpGet("{orderId}")]
public async Task<ActionResult<OrderWithStatus>> GetOrder(int orderId)
{
    if (orderId <= 0)
    {
        return BadRequest("Invalid order ID.");
    }

    var order = await _db.Orders
        .Include(o => o.Pizzas!)
            .ThenInclude(p => p.Special!)
        .Include(o => o.Pizzas!)
            .ThenInclude(p => p.Toppings!)
            .ThenInclude(t => t.Topping!)
        .AsSplitQuery()
        .FirstOrDefaultAsync(o => o.OrderId == orderId);

    if (order == null)
    {
        return NotFound();
    }

    return OrderWithStatus.FromOrder(order);
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