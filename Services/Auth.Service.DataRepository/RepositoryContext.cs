using Auth.Service.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Service.DataRepository
{
    public class AuthServiceDbContext : DbContext
    {
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options)
        {
        }

        public DbSet<AuthClient> AuthClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}