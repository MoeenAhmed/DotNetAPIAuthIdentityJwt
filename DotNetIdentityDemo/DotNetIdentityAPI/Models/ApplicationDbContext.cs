using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentityAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):
            base(dbContextOptions)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
