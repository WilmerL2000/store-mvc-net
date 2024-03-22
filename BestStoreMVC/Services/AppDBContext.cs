using BestStoreMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC.Services
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
