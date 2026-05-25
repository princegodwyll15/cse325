using Microsoft.EntityFrameworkCore;
namespace BlazingPizza.Data;

public class PizzaStoreContext : DbContext
{
    public PizzaStoreContext(
        DbContextOptions options) : base(options)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public DbSet<Pizza> Pizzas { get; set; }

    public DbSet<PizzaSpecial> Specials { get; set; }

    public DbSet<Topping> Toppings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PizzaTopping>()
            .HasKey(pt => new { pt.PizzaId, pt.ToppingId });

        modelBuilder.Entity<PizzaTopping>()
            .HasOne(pt => pt.Pizza)
            .WithMany(p => p.Toppings)
            .HasForeignKey(pt => pt.PizzaId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PizzaTopping>()
            .HasOne(pt => pt.Topping)
            .WithMany()
            .HasForeignKey(pt => pt.ToppingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}