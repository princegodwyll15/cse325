namespace BlazingPizza.Data;

public class OrderWithStatus
{
    public static readonly string[] Statuses = new[]
    {
        "Preparing",
        "Out for delivery",
        "Delivered"
    };

    public Order Order { get; set; } = default!;

    public string StatusText { get; set; } = string.Empty;

    public bool IsDelivered => StatusText == "Delivered";

    public static OrderWithStatus FromOrder(Order order)
    {
        var statusText = (DateTime.Now - order.CreatedTime).TotalMinutes switch
        {
            <= 2 => "Preparing",
            <= 5 => "Out for delivery",
            _ => "Delivered"
        };

        return new OrderWithStatus
        {
            Order = order,
            StatusText = statusText
        };
    }
}