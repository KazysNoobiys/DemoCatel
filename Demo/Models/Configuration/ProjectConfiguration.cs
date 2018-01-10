using System.Data.Entity.ModelConfiguration;
using Demo.Models.DAL;

namespace Demo.Models.Configuration
{
    public class ProjectConfiguration : EntityTypeConfiguration<Project>
    {
        public ProjectConfiguration()
        {
            HasKey(project => project.Id);
            Property(project => project.Customer).IsRequired().HasMaxLength(100);
            Property(project => project.Performer).IsRequired().HasMaxLength(100);
            Property(project => project.Priority).IsRequired();

        }
    }
}