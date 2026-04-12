using Microsoft.EntityFrameworkCore;
using ZaminX.Samples.Data.SqlServer.Domain;

namespace ZaminX.Samples.Data.SqlServer.Infrastructure.Read;

public sealed class AppReadDbContext : DbContext
{
    public AppReadDbContext(DbContextOptions<AppReadDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("Orders");
            builder.HasKey(order => order.Id);

            builder.Property(order => order.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(order => order.Description)
                .HasMaxLength(500);

            builder.Property(order => order.Amount)
                .HasPrecision(18, 2);
        });
    }
}