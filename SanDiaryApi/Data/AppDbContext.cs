using Microsoft.EntityFrameworkCore;
using SanDiaryApi.Models;

namespace SanDiaryApi.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(u => u.PasswordHash)
                      .IsRequired();

                entity.Property(u => u.Role)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasMany(u => u.Notes)
                      .WithOne()
                      .HasForeignKey(n => n.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Note>(entity =>
            {
                entity.HasKey(n => n.Id);

                entity.Property(n => n.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(n => n.Content)
                      .IsRequired();

                entity.Property(n => n.Mood)
                      .HasConversion<string>()
                      .IsRequired();

                entity.Property(n => n.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(n => n.UpdatedAt)
                      .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
