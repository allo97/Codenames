using Microsoft.EntityFrameworkCore;
using TajniacyAPI.JWTAuthentication.Entities;

namespace TajniacyAPI.JWTAuthentication.Helpers
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
