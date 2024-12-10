using AltoCartAPI.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;

namespace AltoCartAPI
{
    public class AltoCartDB : DbContext
    {

        public AltoCartDB()
            : base("name=AltoCartDB")
        {
        }

        public DbSet<Continent> Continents { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<ShopCategory> ShopCategories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<BigPerson> BigPersons { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Owner>()
            //    .HasRequired(o => o.Cities)               // City ile iliþki zorunlu
            //    .WithMany(c => c.Owners)               // City, birden fazla Owner'a sahip
            //    .HasForeignKey(o => o.City_ID)        // Foreign Key tanýmý
            //    .WillCascadeOnDelete(false);          // Silme davranýþý ayarý

            //modelBuilder.Entity<Owner>()
            //    .HasRequired(o => o.Shop)             // Shop ile iliþki opsiyonel
            //    .WithMany(s => s.Owners)              // Shop, birden fazla Owner'a sahip
            //    .HasForeignKey(o => o.Shop_ID)        // Foreign Key tanýmý
            //    .WillCascadeOnDelete(true);           // Shop silindiðinde Owner silinir

            //modelBuilder.Entity<Owner>()
            //    .HasRequired(o => o.Countries)
            //    .WithMany(c => c.Owners)
            //    .HasForeignKey(o => o.Country_ID)
            //    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .HasRequired(o => o.Countries)
                .WithMany(c => c.Owners)
                .HasForeignKey(o => o.Country_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .HasRequired(o => o.Shop)
                .WithMany(s => s.Owners)
                .HasForeignKey(o => o.Shop_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Owner>()
                .HasRequired(o => o.Cities)
                .WithMany(c => c.Owners)
                .HasForeignKey(o => o.City_ID)
                .WillCascadeOnDelete(false);


         //   modelBuilder.Entity<Shop>()
         //.HasRequired(s => s.Countries)  // Countries ile iliþkiyi tanýmlýyoruz.
         //.WithMany(c => c.Shops)
         //.HasForeignKey(s => s.Country_ID)
         //.WillCascadeOnDelete(false);  // Silme iþlemi yapýlmayacak.


            base.OnModelCreating(modelBuilder);
        }
    }
}