using API.Modeles;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Wilaya> Wilayas { get; set; }
        public DbSet<Baladiya> Baladiyas { get; set; }
        public DbSet<ShippingRate> ShippingRates { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShippingRate>()
                .HasOne(s => s.Baladiya)
                .WithMany()
                .HasForeignKey(s => s.BaladiyaId);

            modelBuilder.Entity<Baladiya>()
                .HasOne(b => b.Wilaya)
                .WithMany()
                .HasForeignKey(b => b.WilayaId);

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "اإستراتيجيات" },
                new Category { Id = 2, Name = "الحقائب و الوسائل التعليمية" },
                new Category { Id = 3, Name = "الإدوات المدرسية" },
                new Category { Id = 4, Name = "وسائل المشاريع" },
                new Category { Id = 5, Name = "الأدوات المكتبية" },
                new Category { Id = 6, Name = "الدفاتر" },
                new Category { Id = 7, Name = "وسائل أخرى" }
            );
        }
    }
}
