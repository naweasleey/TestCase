using Microsoft.EntityFrameworkCore;
using TestCase.Models;

namespace TestCase.Data
{
    public class TesCaseContext : DbContext
    {
        public TesCaseContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
