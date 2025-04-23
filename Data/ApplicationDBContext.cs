
using api.Constants;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        public ApplicationDBContext(DbContextOptions options)
       : base(options)
        {
        }

        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Portfolios> Portfolios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // many to many
            //Many-to-Many	AppUser – Stocks (qua Portfolios)
            builder.Entity<Portfolios>(x => x.HasKey(p => new { p.AppUserId, p.StockId }));

            // 1- many : 1 user has many Portfolios 
            // One-to-Many	AppUser – Portfolios
            builder.Entity<Portfolios>()
                .HasOne(u => u.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);

            // 1- many : 1 stock has many Portfolios 
            //One-to-Many	Stock – Portfolios
            builder.Entity<Portfolios>()
                .HasOne(u => u.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);


            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole {
                    Id = "1",
                    Name = RoleUserConstants.ADMIN,
                    NormalizedName = "ADMIN"
                },
                   new IdentityRole {
                    Id = "2",
                    Name = RoleUserConstants.USER,
                    NormalizedName = "USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
