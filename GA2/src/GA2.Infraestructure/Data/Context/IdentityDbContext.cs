using GA2.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GA2.Infraestructure.Data
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<GA2.Domain.Entities.User> USERS { get; set; }
        public DbSet<Rol> ROLES { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
