

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Product.DAL;
public class ProductContext : IdentityDbContext<Users>
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options)
    {

    }
    public DbSet<Product> products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Users>().ToTable("users");
    }
}
