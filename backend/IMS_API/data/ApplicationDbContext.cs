using IMS_API.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();
    public DbSet<SalesOrder> SalesOrders => Set<SalesOrder>();
    public DbSet<SalesOrderItem> SalesOrderItems => Set<SalesOrderItem>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasIndex(p => p.SKU)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<InventoryItem>()
            .HasIndex(i => new { i.ProductId, i.WarehouseId })
            .IsUnique();

        modelBuilder.Entity<PurchaseOrderItem>()
            .HasOne(i => i.PurchaseOrder)
            .WithMany(po => po.Items)
            .HasForeignKey(i => i.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SalesOrderItem>()
            .HasOne(i => i.SalesOrder)
            .WithMany(so => so.Items)
            .HasForeignKey(i => i.SalesOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(80).IsRequired();
        modelBuilder.Entity<Supplier>().Property(s => s.Name).HasMaxLength(120);
        modelBuilder.Entity<Warehouse>().Property(w => w.Name).HasMaxLength(120);

        modelBuilder.Entity<PurchaseOrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);
        modelBuilder.Entity<SalesOrderItem>().Property(x => x.UnitPrice).HasPrecision(18, 2);

        modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "system", FullName = "System" });

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Warehouse>()
            .HasOne(w => w.User)
            .WithMany(u => u.Warehouses)
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<User>().ToTable("Users");
        // Product relationships
        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.CreatedProducts)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);
        // InventoryItem relationships
        modelBuilder.Entity<InventoryItem>()
            .HasOne(i => i.Product)
            .WithMany(p => p.InventoryItems)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(i => i.Warehouse)
            .WithMany(w => w.InventoryItems)
            .HasForeignKey(i => i.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        // PurchaseOrder relationships
        modelBuilder.Entity<PurchaseOrder>()
            .HasOne(po => po.Supplier)
            .WithMany(s => s.PurchaseOrders)
            .HasForeignKey(po => po.SupplierId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseOrder>()
            .Property(po => po.OrderNumber)
            .HasMaxLength(30);

        // SalesOrder relationships
        modelBuilder.Entity<SalesOrder>()
            .HasOne(so => so.Customer)
            .WithMany(c => c.SalesOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SalesOrder>()
            .Property(so => so.OrderNumber)
            .HasMaxLength(30);

        // Customer configuration
        modelBuilder.Entity<Customer>()
            .ToTable("Customers");

        modelBuilder.Entity<Customer>()
            .Property(c => c.Name)
            .HasMaxLength(120);

        modelBuilder.Entity<Customer>()
            .Property(c => c.Email)
            .HasMaxLength(100);

        modelBuilder.Entity<Customer>()
            .Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        modelBuilder.Entity<Customer>()
            .Property(c => c.Address)
            .HasMaxLength(200);

        modelBuilder.Entity<Customer>()
            .HasOne(c => c.Warehouse)
            .WithMany(w => w.Customers)
            .HasForeignKey(c => c.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Supplier configuration
        modelBuilder.Entity<Supplier>()
            .Property(s => s.Email)
            .HasMaxLength(100);

        modelBuilder.Entity<Supplier>()
            .Property(s => s.PhoneNumber)
            .HasMaxLength(20);

        modelBuilder.Entity<Supplier>()
            .Property(s => s.Address)
            .HasMaxLength(200);
        modelBuilder.Entity<Supplier>()
            .Property(s => s.Name)
            .HasMaxLength(120);
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.Warehouse)
            .WithMany(w => w.Suppliers)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product Name constraint
        modelBuilder.Entity<Product>()
            .Property(p => p.Name)
            .HasMaxLength(100);

        // Product SKU constraint
        modelBuilder.Entity<Product>()
            .Property(p => p.SKU)
            .HasMaxLength(50);

        // category Name uniqueness
        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
        // User Username uniqueness
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
