using BookStore_MVC_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore_MVC_Web.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}