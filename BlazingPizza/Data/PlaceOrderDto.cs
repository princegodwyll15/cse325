namespace BlazingPizza.Data;

public class PlaceOrderDto
{
    public List<PlaceOrderPizzaDto> Pizzas { get; set; } = new();
}

public class PlaceOrderPizzaDto
{
    public int SpecialId { get; set; }

    public int Size { get; set; }
}
