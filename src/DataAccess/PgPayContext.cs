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

            modelBuilder.Entity<ExpenceCategory>()
                .HasMany(c => c.Users)
                .WithMany(u => u.ExpenceCategories);
        }
    }
}
