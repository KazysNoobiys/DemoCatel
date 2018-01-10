using System.Data.Entity;
using Demo.Models.DAL;
using Orc.EntityFramework.Repositories;

namespace Demo.Models.Repository
{
    public class EmployeeRepository:EntityRepositoryBase<Employee,int>, IEmployeeRepository
    {
        public EmployeeRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public interface IEmployeeRepository:IEntityRepository<Employee,int>
    {
    }
}