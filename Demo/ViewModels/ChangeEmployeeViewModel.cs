using System.Collections.ObjectModel;
using System.Linq;
using Catel.Data;
using Demo.Models.DAL;
using Demo.Models.Repository;
using Orc.EntityFramework;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class ChangeEmployeeViewModel : ViewModelBase
    {
        private ObservableCollection<Project> _projects;
        public override string Title { get { return "Изменение сотрудника"; } }
        public ChangeEmployeeViewModel(int employeeId)
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var employeeRep = uow.GetRepository<IEmployeeRepository>();
                
                Employee = employeeRep.Find(employee => employee.Id == employeeId).First();
            }
        }

        #region Command

        #region Check command

        private Command _checkCommand;

        /// <summary>
        /// Gets the Check command.
        /// </summary>
        public Command CheckCommand
        {
            get { return _checkCommand ?? (_checkCommand = new Command(Check)); }
        }

        /// <summary>
        /// Method to invoke when the Check command is executed.
        /// </summary>
        private void Check()
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

        #endregion

        #region UnCheck command

        private Command _unCheckCommand;

        /// <summary>
        /// Gets the UnCheck command.
        /// </summary>
        public Command UnCheckCommand
        {
            get { return _unCheckCommand ?? (_unCheckCommand = new Command(UnCheck)); }
        }

        /// <summary>
        /// Method to invoke when the UnCheck command is executed.
        /// </summary>
        private void UnCheck()
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

        #endregion

        #endregion

        #region Property

        #region Employee property

        /// <summary>
        /// Gets or sets the Employee value.
        /// </summary>
        [Model(SupportIEditableObject = false)]
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

        #endregion
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LeaderProjects = new ObservableCollection<LeaderProject>();
            IncludProjects = new ObservableCollection<IncludeProject>();

            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var projectRep = uow.GetRepository<IProjectRepository>();

                _projects = new ObservableCollection<Project>(projectRep.GetAll());

                foreach (var project in _projects)
                {
                    if (Employee.LeaderToProjects.Contains(project))
                    {
                        LeaderProjects.Add(new LeaderProject(project,true));
                    }
                    else
                    {
                        LeaderProjects.Add(new LeaderProject(project, false));
                        if (Employee.Projects.Contains(project))
                        {
                            IncludProjects.Add(new IncludeProject(project,true));
                        }
                        else
                        {
                            IncludProjects.Add(new IncludeProject(project, false));
                        }
                    }
                    
                }
            }
        }

        protected override Task<bool> SaveAsync()
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var employeeRep = uow.GetRepository<IEmployeeRepository>();
                var projectRep = uow.GetRepository<IProjectRepository>();

                foreach (var leaderProject in LeaderProjects)
                {
                    if (leaderProject.IsLeader)
                    {
                        if (!Employee.LeaderToProjects.Contains(leaderProject.Project))
                        {
                            if (leaderProject.Project.Leader == null)
                            {
                                Employee.LeaderToProjects.Add(leaderProject.Project);
                                Employee.Projects.Add(leaderProject.Project);
                                leaderProject.Project.Leader = Employee;
                                leaderProject.Project.Employees.Add(Employee);

                                employeeRep.Update(Employee);
                                projectRep.Update(leaderProject.Project);
                            }
                            else
                            {
                                leaderProject.Project.Leader.LeaderToProjects.Remove(leaderProject.Project);
                                leaderProject.Project.Employees.Remove(leaderProject.Project.Leader);
                                employeeRep.Update(leaderProject.Project.Leader);

                                leaderProject.Project.Leader = Employee;
                                leaderProject.Project.Employees.Add(Employee);
                                Employee.LeaderToProjects.Add(leaderProject.Project);
                                Employee.Projects.Add(leaderProject.Project);

                                employeeRep.Update(Employee);
                                projectRep.Update(leaderProject.Project);
                            }
                            
                        }
                    }
                    else
                    {
                        if (Employee.LeaderToProjects.Contains(leaderProject.Project))
                        {
                            leaderProject.Project.Leader = null;
                            leaderProject.Project.Employees.Remove(Employee);

                            Employee.LeaderToProjects.Remove(leaderProject.Project);
                            Employee.Projects.Remove(leaderProject.Project);

                            employeeRep.Update(Employee);
                            projectRep.Update(leaderProject.Project);
                        }
                    }
                }

                foreach (var includeProject in IncludProjects)
                {
                    if (includeProject.InProject)
                    {
                        if (!Employee.Projects.Contains(includeProject.Project))
                        {
                            includeProject.Project.Employees.Add(Employee);
                            Employee.Projects.Add(includeProject.Project);
                            projectRep.Update(includeProject.Project);
                            employeeRep.Update(Employee);
                        }
                    }
                    else
                    {
                        if (Employee.Projects.Contains(includeProject.Project))
                        {
                            includeProject.Project.Employees.Remove(Employee);
                            Employee.Projects.Remove(includeProject.Project);
                            projectRep.Update(includeProject.Project);
                            employeeRep.Update(Employee);
                        }
                    }
                }

                employeeRep.Update(Employee);
                uow.SaveChanges();
            }

            return base.SaveAsync();
        }
    }
}
