using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{ 
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
    
    public DbSet<User>? Users { get; set; }
    public DbSet<Application>? Applications { get; set; }
}