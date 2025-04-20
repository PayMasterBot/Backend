using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PgPayContext : DbContext
    {
        public DbSet<ExchangeRateSubscription> ExchangeSubs { get; set; }
        public DbSet<Expence> Expences { get; set; }
        public DbSet<ExpenceCategory> ExpenceCategories { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }

        public PgPayContext(DbContextOptions<PgPayContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExchangeRateSubscription>()
                .HasKey(s => new { s.UserId, s.Currency1, s.Currency2 });

            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.User)
                .WithMany(u => u.Subscriptions)
                .HasForeignKey(s => s.UserId)
                .HasPrincipalKey(u => u.Id);
            modelBuilder.Entity<Expence>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expences)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(u => u.Id);
            modelBuilder.Entity<Expence>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expences)
                .HasForeignKey(e => e.CategoryId)
                .HasPrincipalKey(c => c.Id);
            modelBuilder.Entity<ExchangeRateSubscription>()
                .HasOne(e => e.User)
                .WithMany(u => u.ExchangeRates)
                .HasForeignKey(e => e.UserId)
                .HasPrincipalKey(u => u.Id);
            modelBuilder.Entity<ExpenceCategory>()
                .HasMany(c => c.Users)
                .WithMany(u => u.ExpenceCategories);
        }
    }
}
