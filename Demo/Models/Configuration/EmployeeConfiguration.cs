using System.Data.Entity.ModelConfiguration;
using Demo.Models.DAL;

namespace Demo.Models.Configuration
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            HasKey(employee => employee.Id);
            Property(employee => employee.FirstName).IsRequired().HasMaxLength(50);
            Property(employee => employee.LastName).IsRequired().HasMaxLength(50);
            Property(employee => employee.Patronymic).HasMaxLength(50);
            Property(employee => employee.Email).HasMaxLength(50);

            HasMany(employee => employee.Projects)
                .WithMany(project => project.Employees)
                .Map(m =>
                {
                    m.MapLeftKey("EmpId");
                    m.MapRightKey("ProjId");
                    m.ToTable("ProjectsToEmployees");
                });

            HasMany(employee => employee.LeaderToProjects)
                .WithOptional(project => project.Leader)
                .WillCascadeOnDelete(false);
        }
    }
}