using Demo.Models.DAL;

namespace Demo.ViewModels
{
    public class LeaderProject
    {
        public Project Project { get; set; }
        public bool IsLeader { get; set; }

        public LeaderProject(Project project, bool isLeader)
        {
            Project = project;
            IsLeader = isLeader;
        }
    }
}