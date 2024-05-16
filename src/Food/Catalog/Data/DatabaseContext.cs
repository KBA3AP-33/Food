using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Catalog.Data
{
    public class DatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TimeCategory> TimeCategories { get; set; }
        public DbSet<Kitchen> Kitchens { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<IngredientCount> IngredientCounts { get; set; }


        public DatabaseContext()
        {

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Recipe>()
                .HasOne(e => e.Author)
                .WithMany(e => e.Recipes);

            builder.Entity<Recipe>()
                .HasMany(c => c.UsersLikes)
                .WithMany(s => s.FavouriteRecipes);

            builder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(e => e.Recipe)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Recipe>()
                .HasMany(r => r.UserRatings)
                .WithOne(e => e.Recipe)
                .OnDelete(DeleteBehavior.Cascade);

            var index = new Sources.Index();
            if (index.Filters != null && index.Users != null && index.Recipes != null && index.Ingredients != null)
            {
                builder.Entity<Category>().HasData(index.Filters.Categories!);
                builder.Entity<TimeCategory>().HasData(index.Filters.TimeCategories!);
                builder.Entity<Ingredient>().HasData(index.Filters.Ingredients!);
                builder.Entity<Status>().HasData(index.Filters.Statuses!);
                builder.Entity<Kitchen>().HasData(index.Filters.Kitchens!);
                builder.Entity<IdentityRole>().HasData(index.Users.Roles!);
                builder.Entity<User>().HasData(index.Users.Users!);
                builder.Entity<IdentityUserRole<string>>().HasData(index.Users.UserRoles!);
                builder.Entity<Recipe>().HasData(index.Recipes!);
                builder.Entity<IngredientCount>().HasData(index.Ingredients!);
            }
        }
    }
}
