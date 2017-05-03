using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using SomeStore.Infrustructure.DbEntities;
using SomeStore.Models;

namespace SomeStore.Infrustructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
            Database.SetInitializer(new AppDbInitializer());
        }

        #region DbSets
        public DbSet<Product> Products { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .ToTable("Products");

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.ShoppingCarts)
                .WithOptional()
                .WillCascadeOnDelete();

            modelBuilder.Entity<ShoppingCart>()
                .ToTable("ShoppingCarts")
                .HasMany(x => x.CartItems)
                .WithOptional()
                .WillCascadeOnDelete();

            modelBuilder.Entity<CartItem>()
                .ToTable("CartItems")
                .HasRequired(x => x.Product);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}