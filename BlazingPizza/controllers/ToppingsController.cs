using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazingPizza.Data;

namespace BlazingPizza.Controllers;

[Route("toppings")]
[ApiController]
public class ToppingsController : Controller
{
    private readonly PizzaStoreContext _db;

    public ToppingsController(PizzaStoreContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Topping>>> GetToppings()
    {
        return (await _db.Toppings.ToListAsync()).OrderByDescending(t => t.Price).ToList();
    }  
}   