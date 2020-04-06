using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleaningProject.Models
{
    public class CleaningUserDbContext:IdentityDbContext<CleaningUser>
    {
        public CleaningUserDbContext(DbContextOptions<CleaningUserDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<CleaningUser>(user => user.HasIndex(x => x.Fullname).IsUnique(false));
            builder.Entity<CleaningUser>(user => user.HasIndex(x => x.Created).IsUnique(false));

            builder.Entity<ServiceTypeRecord>()
                .HasKey(t => new { t.ServiceId, t.ServiceTypeId });

            builder.Entity<ServiceTypeRecord>()
                .HasOne(p => p.po)
                .WithMany(p => p.serviceType)
                .HasForeignKey(y => y.ServiceId);

            builder.Entity<ServiceTypeRecord>()
                .HasOne(p => p.qo)
                .WithMany(p => p.service)
                .HasForeignKey(y => y.ServiceTypeId);
        }

        public DbSet<Company> company { get; set; }
        public DbSet<DaysOfWeek> days { get; set; }
        public DbSet<UserImageRecord> UserImg { get; set; }
        public DbSet<Equipment> equipment { get; set; }

        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceRecord> ServiceType { get; set; }
        public DbSet<ServiceTypeRecord> ServiceTypeRecord { get; set; }
        public DbSet<ServiceRequest> ServiceRequest { get; set; }
        public DbSet<CleaningItem>CleaningItem { get; set; }
        public DbSet<Team> team { get; set; }
        public DbSet<ConfigureTeam> ConfigureTeam { get; set; }
        public DbSet<EquipmentTracking> EquipmentTracking { get; set; }
        public DbSet<Invoice> Invoice { get; set; }

    }
}
