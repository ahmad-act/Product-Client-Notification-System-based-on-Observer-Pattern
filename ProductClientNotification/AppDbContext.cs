using Microsoft.EntityFrameworkCore;
using ProductClientNotification.Models;

/* Ensure Migration and Database Initialization:

Open Package Manager Console:
Navigate to Tools > NuGet Package Manager > Package Manager Console in Visual Studio.

Execute the following command:
    Add-Migration InitialCreate
    Update-Database

*/

namespace ProductClientNotification
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ProductClientNotification.db");
        }

    }
}
