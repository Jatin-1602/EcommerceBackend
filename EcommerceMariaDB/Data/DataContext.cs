using Microsoft.EntityFrameworkCore;
using EcommerceMariaDB.Models;

namespace EcommerceMariaDB.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<TrackOrder> TrackOrders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOTP> UserOTPs { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<SellerProduct> SellerProducts { get; set; }
        public DbSet<SellerOrder> SellerOrders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany<Product>()
                .WithOne()
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey(oi => oi.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany<SellerProduct>()
                .WithOne()
                .HasForeignKey(sp => sp.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Order>()
               .HasMany<OrderItem>()
               .WithOne()
               .HasForeignKey(oi => oi.OrderId)
               .IsRequired();



            modelBuilder.Entity<OrderItem>()
                .HasOne<TrackOrder>()
                .WithOne()
                .HasForeignKey<TrackOrder>(to => to.OrderItemId)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne<SellerOrder>()
                .WithOne()
                .HasForeignKey<SellerOrder>(so => so.OrderItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);




            modelBuilder.Entity<Payment>()
                .HasOne<Order>()
                .WithOne()
                .HasForeignKey<Order>(o => o.RazorpayPaymentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);




            modelBuilder.Entity<User>()
                .HasMany<Payment>()
                .WithOne()
                .HasForeignKey(p => p.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasMany<Order>()
                .WithOne()
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasOne<Cart>()
                .WithOne()
                .HasForeignKey<Cart>(u => u.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasOne<UserOTP>()
                .WithOne()
                .HasForeignKey<UserOTP>(uo => uo.UserId)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasOne<Seller>()
                .WithOne()
                .HasForeignKey<Seller>(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);



            modelBuilder.Entity<Cart>()
                .HasMany<CartItem>()
                .WithOne()
                .HasForeignKey(ci => ci.CartId)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .HasMany<CartItem>()
                .WithOne()
                .HasForeignKey(c => c.ProductId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);




            modelBuilder.Entity<Seller>()
                .HasMany<SellerProduct>()
                .WithOne()
                .HasForeignKey(sp => sp.SellerId)
                .IsRequired();

            modelBuilder.Entity<Seller>()
                .HasMany<SellerOrder>()
                .WithOne()
                .HasForeignKey(so => so.SellerId)
                .IsRequired();

        }
    }
}