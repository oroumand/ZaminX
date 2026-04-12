using Microsoft.EntityFrameworkCore;
using ZaminX.BuildingBlocks.Data.EntityFrameworkCore.Extensions;
using ZaminX.Samples.Data.PostgreSql.Domain;

namespace ZaminX.Samples.Data.PostgreSql.Infrastructure.Write;

public sealed class AppWriteDbContext : DbContext
{
    public AppWriteDbContext(DbContextOptions<AppWriteDbContext> options)
        : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("orders");
            builder.HasKey(order => order.Id);

            builder.Property(order => order.Id)
                .ValueGeneratedOnAdd();

            builder.Property(order => order.CustomerName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(order => order.Description)
                .HasMaxLength(500);

            builder.Property(order => order.Amount)
                .HasPrecision(18, 2);
        });

        modelBuilder.AddAuditShadowProperties();
    }
}