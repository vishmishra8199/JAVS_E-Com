using System.Reflection.Metadata.Ecma335;
using JWT_Token_Example.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT_Token_Example.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Vendor> Vendors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Vendor>().ToTable("vendors");
    }
}