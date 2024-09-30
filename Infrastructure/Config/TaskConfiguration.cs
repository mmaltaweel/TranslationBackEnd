using Core.Enities.ProjectAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Config;

public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.Status)
            .IsRequired();

        builder.Property(t => t.DueDate)
            .IsRequired();
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Property(t => t.Description).HasMaxLength(500);
        builder.Property(t => t.DueDate).IsRequired();
        builder.Property(t => t.Status).IsRequired();
        
        builder.HasOne(t => t.AssignedTranslator)
            .WithMany(u => u.AssignedTasks)
            .HasForeignKey(t => t.AssignedTranslatorId)
            .OnDelete(DeleteBehavior.Restrict);
    } 
}