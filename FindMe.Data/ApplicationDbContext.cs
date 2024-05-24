using FindMe.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FindMe.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Form> Forms { get; set; }
    public DbSet<SimpRequest> SimpRequests { get; set; }
    public DbSet<SuccessfulSimpRequest> SuccessfulSimpRequests { get; set; }
    public DbSet<ViewedForms> ViewedForms { get; set; }
    public DbSet<UserInterest> UserInterests { get; set; }
    public DbSet<Interest> Interests { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserInterest>()
            .HasOne(ui => ui.Interest)
            .WithMany()
            .HasForeignKey(ui => ui.InterestId);
    }
}