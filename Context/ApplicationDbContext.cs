using FoodApiService.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodApiService.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Food> Foods { get; set; } = default!;
        public DbSet<FoodImage> FoodImages { get; set; } = default!;



        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>()
                .HasMany(f => f.FoodImages)
                .WithOne(fi => fi.Food)
                .HasForeignKey(fi => fi.FoodId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Food>().Property(x=>x.Name).HasMaxLength(50).IsRequired();
            modelBuilder.Entity<Food>().Property(x => x.Description).HasMaxLength(200).IsRequired();
            modelBuilder.Entity<Food>().Property(x => x.Price).HasColumnType("decimal(18,2)").IsRequired();
            modelBuilder.Entity<Food>().Property(x => x.CategoryId).IsRequired();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseEntity);
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Modified)
                {
                    ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
                }
                if (entry.State == EntityState.Deleted)
                {
                    ((BaseEntity)entry.Entity).IsDeleted = true;
                    ((BaseEntity)entry.Entity).DeletedAt = DateTime.UtcNow;
                    entry.State = EntityState.Modified;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
