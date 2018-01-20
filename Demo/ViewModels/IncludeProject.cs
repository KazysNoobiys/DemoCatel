using Demo.Models.DAL;

namespace Demo.ViewModels
{
    public class IncludeProject
    {
        public Project Project { get; set; }
        public bool InProject { get; set; }

        public IncludeProject(Project project, bool inProject)
        {
            Project = project;
            InProject = inProject;
        }
    }
}