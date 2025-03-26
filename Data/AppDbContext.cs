using ChecklistApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChecklistApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Checklist> Checklists { get; set; }
        public DbSet<ChecklistItem> ChecklistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração do relacionamento
            modelBuilder.Entity<ChecklistItem>()
                .HasOne(ci => ci.Checklist)
                .WithMany(c => c.Itens)
                .HasForeignKey(ci => ci.ChecklistId);
        }
    }
}