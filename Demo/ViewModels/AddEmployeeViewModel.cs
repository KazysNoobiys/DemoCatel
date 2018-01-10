using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Data;
using Catel.Services;
using Demo.Models.DAL;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

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
    public class AddEmployeeViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Project> _projects;
        private readonly IMessageService _messageService;

        public AddEmployeeViewModel(Employee employee, ObservableCollection<Project> projects, IMessageService messageService)
        {
            _projects = projects;
            _messageService = messageService;
            Employee = employee;

            CheckCommand = new Command(CheckCommandExecute);
            UnCheckCommand = new Command(UnCheckCommandExecute);

        }

        public Command CheckCommand { get; private set; }
        public Command UnCheckCommand { get; private set; }

        private void CheckCommandExecute()
        {
            IncludeProject temp = null;
            foreach (var leaderProject in LeaderProjects)
            {
                if (leaderProject.IsLeader)
                {
                    foreach (var includeProject in IncludProjects)
                    {
                        if (includeProject.Project == leaderProject.Project)
                        {
                            temp = includeProject;
                            break; ;
                        }
                    }
                }
            }

            if (temp != null)
                IncludProjects.Remove(temp);


        }
        private void UnCheckCommandExecute()
        {
            foreach (var leaderProject in LeaderProjects)
            {
                if (!leaderProject.IsLeader)
                {
                    bool find = false;
                    foreach (var includeProject in IncludProjects)
                    {
                        if (includeProject.Project == leaderProject.Project)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        IncludProjects.Add(new IncludeProject(leaderProject.Project, false));
                    }
                }
            }
        }

        public override string Title { get { return "Новый сотрудник"; } }

        #region IncludProjects property

        /// <summary>
        /// Gets or sets the IncludProjects value.
        /// </summary>
        public ObservableCollection<IncludeProject> IncludProjects
        {
            get { return GetValue<ObservableCollection<IncludeProject>>(IncludProjectsProperty); }
            set { SetValue(IncludProjectsProperty, value); }
        }

        /// <summary>
        /// IncludProjects property data.
        /// </summary>
        public static readonly PropertyData IncludProjectsProperty =
            RegisterProperty("IncludProjects", typeof(ObservableCollection<IncludeProject>));

        #endregion

        #region LeaderProjects property

        /// <summary>
        /// Gets or sets the LeaderProjects value.
        /// </summary>
        public ObservableCollection<LeaderProject> LeaderProjects
        {
            get { return GetValue<ObservableCollection<LeaderProject>>(LeaderProjectsProperty); }
            set { SetValue(LeaderProjectsProperty, value); }
        }

        /// <summary>
        /// LeaderProjects property data.
        /// </summary>
        public static readonly PropertyData LeaderProjectsProperty =
            RegisterProperty("LeaderProjects", typeof(ObservableCollection<LeaderProject>));



        #endregion

        #region Employee property

        /// <summary>
        /// Gets or sets the Employee value.
        /// </summary>
        [Model]
        public Employee Employee
        {
            get { return GetValue<Employee>(EmployeeProperty); }
            set { SetValue(EmployeeProperty, value); }
        }

        /// <summary>
        /// Employee property data.
        /// </summary>
        public static readonly PropertyData EmployeeProperty = RegisterProperty("Employee", typeof(Employee));

        #endregion

        #region FirstName property

        /// <summary>
        /// Gets or sets the FirstName value.
        /// </summary>
        [ViewModelToModel]
        public string FirstName
        {
            get { return GetValue<string>(FirstNameProperty); }
            set { SetValue(FirstNameProperty, value); }
        }

        /// <summary>
        /// FirstName property data.
        /// </summary>
        public static readonly PropertyData FirstNameProperty = RegisterProperty("FirstName", typeof(string));

        #endregion

        #region LastName property

        /// <summary>
        /// Gets or sets the LastName value.
        /// </summary>
        [ViewModelToModel]
        public string LastName
        {
            get { return GetValue<string>(LastNameProperty); }
            set { SetValue(LastNameProperty, value); }
        }

        /// <summary>
        /// LastName property data.
        /// </summary>
        public static readonly PropertyData LastNameProperty = RegisterProperty("LastName", typeof(string));

        #endregion

        #region Patronymic property

        /// <summary>
        /// Gets or sets the Patronymic value.
        /// </summary>
        [ViewModelToModel]
        public string Patronymic
        {
            get { return GetValue<string>(PatronymicProperty); }
            set { SetValue(PatronymicProperty, value); }
        }

        /// <summary>
        /// Patronymic property data.
        /// </summary>
        public static readonly PropertyData PatronymicProperty = RegisterProperty("Patronymic", typeof(string));

        #endregion

        #region Email property

        /// <summary>
        /// Gets or sets the Email value.
        /// </summary>
        [ViewModelToModel]
        public string Email
        {
            get { return GetValue<string>(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }

        /// <summary>
        /// Email property data.
        /// </summary>
        public static readonly PropertyData EmailProperty = RegisterProperty("Email", typeof(string));

        #endregion



        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LeaderProjects = new ObservableCollection<LeaderProject>();
            IncludProjects = new ObservableCollection<IncludeProject>();
            foreach (var project in _projects)
            {
                if (project.Leader == null)
                    LeaderProjects.Add(new LeaderProject(project, false));
                IncludProjects.Add(new IncludeProject(project, false));
            }
        }

        protected override async Task CloseAsync()
        {
            foreach (var project in LeaderProjects)
            {
                if (project.IsLeader)
                {
                    Employee.LeaderToProjects.Add(project.Project);
                }
            }
            foreach (var project in IncludProjects)
            {
                if (project.InProject)
                {
                    Employee.Projects.Add(project.Project);
                }
            }

            await base.CloseAsync();
        }
    }
}
