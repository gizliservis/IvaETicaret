using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IvaETicaret.Models;

namespace IvaETicaret.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<IvaETicaret.Models.Category> Categories { get; set; }
        public DbSet<IvaETicaret.Models.Department> Departments { get; set; }
        public DbSet<IvaETicaret.Models.Product> Products { get; set; }
        public DbSet<IvaETicaret.Models.Branch> Branches { get; set; }
        public DbSet<IvaETicaret.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}