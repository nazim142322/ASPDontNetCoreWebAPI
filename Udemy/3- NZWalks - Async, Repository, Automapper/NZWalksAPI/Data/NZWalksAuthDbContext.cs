using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {
        }
        // Add DbSet properties for your entities here if needed
        // For example:
        // public DbSet<YourEntity> YourEntities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //seed data for roles
            var readerRoleId = "3d878464-81c8-481f-92cc-48d3efc8ee0b".ToString();
            var writerRoleId = "c5715846-7a07-4fba-bc3e-bfd9dcdd67a9".ToString();
            var roles = new List<IdentityRole>()
            {
                new IdentityRole() { Id= readerRoleId, ConcurrencyStamp=readerRoleId, Name = "Reader", NormalizedName="Reader".ToUpper() },
                new IdentityRole() { Id = writerRoleId, ConcurrencyStamp=writerRoleId,  Name="Writer", NormalizedName="Writer".ToUpper() }
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
