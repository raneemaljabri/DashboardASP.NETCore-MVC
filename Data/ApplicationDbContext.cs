using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Products> products { get; set; }
        public DbSet<ProductsDetails> productsDetails { get; set; }
        public DbSet<DamegedProducts> damegedproducts { get; set; }

    }
}

