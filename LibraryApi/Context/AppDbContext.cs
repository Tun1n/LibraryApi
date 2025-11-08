using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection;

namespace LibraryApi.Context
{
    public class AppDbContext : DbContext
    {
     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book>? Books { get; set; }
    public DbSet<Genre>? Genres { get; set; }
        
    }
}
