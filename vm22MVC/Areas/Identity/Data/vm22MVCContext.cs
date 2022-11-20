using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using vm22MVC.Models;

namespace vm22MVC.Data;

public class vm22MVCContext : IdentityDbContext<IdentityUser>
{
    public vm22MVCContext(DbContextOptions<vm22MVCContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<vm22MVC.Models.TournamentModel> TournamentModel { get; set; }
}
