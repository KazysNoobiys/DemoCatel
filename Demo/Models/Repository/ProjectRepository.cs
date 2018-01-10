using System.Data.Entity;
using Demo.Models.DAL;
using Orc.EntityFramework.Repositories;

namespace Demo.Models.Repository
{
    public class ProjectRepository:EntityRepositoryBase<Project,int>, IProjectRepository
    {
        public ProjectRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }

    public interface IProjectRepository:IEntityRepository<Project,int>
    {
    }
}