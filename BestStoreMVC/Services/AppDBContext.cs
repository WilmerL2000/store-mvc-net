using Microsoft.EntityFrameworkCore;

namespace BestStoreMVC.Services
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
