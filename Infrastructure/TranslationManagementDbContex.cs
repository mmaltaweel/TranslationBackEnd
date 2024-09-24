using System.Reflection;
using Core.Enities.ProjectAggregate;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
 
public class TranslationManagementDbContext:  IdentityDbContext<User>
{
    public TranslationManagementDbContext(DbContextOptions<TranslationManagementDbContext> options) : base(options)
    {

    } 
    public TranslationManagementDbContext() : base()
    {

    }   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
         if (!optionsBuilder.IsConfigured)
         {
             optionsBuilder.UseSqlServer("");
         }
         optionsBuilder.UseLazyLoadingProxies();
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
}