using Microsoft.EntityFrameworkCore;

namespace AspirePostgresMigrationIssue.Data;
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students { get; set; } 
}
