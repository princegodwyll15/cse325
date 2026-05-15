using Microsoft.AspNetCore.Rewrite;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//register the todo service as a singleton
builder.Services.AddSingleton<ITodoService>(new InMemoryTodoService());

var app = builder.Build();

//add middleware to tranform route paths
app.UseRewriter(new RewriteOptions().AddRedirect("task/(.*)", "todos/$1"));

//custome middleware to log request and response
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request path: {context.Request.Method} {context.Request.Path} {DateTime.Now} started");
    await next(context);
    Console.WriteLine($"Response status code: {context.Response.StatusCode} {DateTime.Now} finished");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};


app.MapPost("/todos", (Todo todo, ITodoService todoService) =>
{
    todoService.AddTodo(todo);
    return TypedResults.Created($"/todos/{todo.Id}", todo);
})
.AddEndpointFilter(async (context, next) => // Validation filter 
{
    var todo = context.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();
    if (todo is null || string.IsNullOrEmpty(todo.Name))
    {
        errors.Add(nameof(todo.Name), ["Name is required"]);
    }
    if (todo.Duetime < DateTime.Now)
    {
        errors.Add(nameof(todo.Duetime), ["Due time must be in the future"]);
    }
    if (todo.Iscomplete && string.IsNullOrEmpty(todo.Description))
    {
        errors.Add(nameof(todo.Description), ["Description is required when the task is complete"]);
    }

    if (todo.Iscomplete)
    {
        errors.Add(nameof(todo.Iscomplete), ["Task cannot be marked as complete when created"]);
    }
    if (errors.Count > 0)
    {
        return Results.ValidationProblem(errors);
    }
    return await next(context);
});

//get todo by id
app.MapGet("/todos/{id}", (int id, ITodoService todoService) =>
{
    var targetTodo = todoService.GetTodoById(id);
    return targetTodo is null ? Results.NotFound() : Results.Ok(targetTodo);
});

//delete todo by id
app.MapDelete("/todos/{id}", (int id, ITodoService todoService) =>
{
    var deleted = todoService.DeleteTodoById(id);
    return deleted ? Results.NoContent() : Results.NotFound();
});

// Get all todos using the todo service
app.MapGet("/todos", (ITodoService todoService) => todoService.GetAllTodos());

app.MapPut("/todos/{id}", (int id, Todo updatedTodo, ITodoService todoService) =>
{
    var existingTodo = todoService.GetTodoById(id);
    if (existingTodo is null)
    {
        return Results.NotFound();
    }
    if (updatedTodo.Duetime < DateTime.Now)
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { nameof(updatedTodo.Duetime), ["Due time must be in the future"] }
        });
    }
    if (updatedTodo.Iscomplete && string.IsNullOrEmpty(updatedTodo.Description))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>
        {
            { nameof(updatedTodo.Description), ["Description is required when the task is complete"] }
        });
    }
    var newTodo = existingTodo with
    {
        Name = updatedTodo.Name,
        Duetime = updatedTodo.Duetime,
        Iscomplete = updatedTodo.Iscomplete,
        Description = updatedTodo.Description
    };
    todoService.DeleteTodoById(id);
    todoService.AddTodo(newTodo);
    return Results.Ok(newTodo);
});


app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Todo(int Id, string Name, DateTime Duetime, bool Iscomplete, string? Description = null);

//create a todo interface
interface ITodoService
{
    void AddTodo(Todo todo);
    Todo? GetTodoById(int id);
    bool DeleteTodoById(int id);
    IEnumerable<Todo> GetAllTodos();
}

class InMemoryTodoService : ITodoService
{
    private readonly List<Todo> _todos = new();

    public void AddTodo(Todo todo)
    {
        _todos.Add(todo);
    }

    public Todo? GetTodoById(int id)
    {
        return _todos.SingleOrDefault(t => t.Id == id);
    }

    public bool DeleteTodoById(int id)
    {
        return _todos.RemoveAll(t => t.Id == id) > 0;
    }

    public IEnumerable<Todo> GetAllTodos()
    {
        return _todos;
    }
}