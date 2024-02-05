using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DbProject.Contexts;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }


    public DbSet<RoleEntity> Roles { get; set; }
    public DbSet<AddressEntity> Address { get; set; }
    public DbSet<CustomerEntity> Customer { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerEntity>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<RoleEntity>().HasIndex(x => x.RoleName).IsUnique();


        base.OnModelCreating(modelBuilder);
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Skapa en logger-fabrik och filtrera bort vissa loggmeddelanden
        ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information).ClearProviders();
        });

        optionsBuilder
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging(); 
    }
}
