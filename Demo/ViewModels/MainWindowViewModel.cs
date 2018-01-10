using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;
using Catel.Data;
using Catel.IoC;
using Catel.Services;
using Demo.Models.DAL;
using Demo.Models.Repository;
using Orc.EntityFramework;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;
    using Models;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;

        public MainWindowViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var projectRep = uow.GetRepository<IProjectRepository>();
                var employeeRep = uow.GetRepository<IEmployeeRepository>();

                var projects = new ObservableCollection<Project>(projectRep.GetAll()
                    .Include("Leader").Include("Employees"));

                var employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                            
                Projects = CollectionViewSource.GetDefaultView(projects);
                Employees = CollectionViewSource.GetDefaultView(employees);

            }
        }
        
        public override string Title { get { return "DemoCatel"; } }

        #region Add command

        private TaskCommand _addCommand;

        /// <summary>
        /// Gets the Add command.
        /// </summary>
        public TaskCommand AddCommand
        {
            get { return _addCommand ?? (_addCommand = new TaskCommand(Add)); }
        }

        /// <summary>
        /// Method to invoke when the Add command is executed.
        /// </summary>
        private async Task Add()
        {
            switch (SelectedTabIndex)
            {
                case (0):
                    await AddProject();
                    break;
                case (1):
                    await AddEmployee();
                    break;
                default:
                    throw new Exception("Ошибка");

            }
            
        }

        private async Task AddEmployee()
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var projectRep = uow.GetRepository<IProjectRepository>();
                var projects = new ObservableCollection<Project>(projectRep.GetAll());

                var newEmployee = new Employee();
                AddEmployeeViewModel modalWindow = new AddEmployeeViewModel(newEmployee, projects, _messageService);
                if (await _uiVisualizerService.ShowDialogAsync(modalWindow) == true)
                {
                    var employeeRep = uow.GetRepository<IEmployeeRepository>();
                    employeeRep.Add(newEmployee);

                    if (newEmployee.LeaderToProjects != null)
                    {
                        foreach (var project in newEmployee.LeaderToProjects)
                        {
                            projectRep.Update(project);
                        }
                    }
                    if (newEmployee.Projects != null)
                    {
                        foreach (var project in newEmployee.Projects)
                        {
                            projectRep.Update(project);
                        }
                    }

                    uow.SaveChanges();
                    Employees = CollectionViewSource.GetDefaultView(employeeRep.GetAll());
                    Projects = CollectionViewSource.GetDefaultView(projectRep.GetAll());
                }
            }
        }

        private async Task AddProject()
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var employeeRep = uow.GetRepository<IEmployeeRepository>();
                var employees = new ObservableCollection<Employee>(employeeRep.GetAll());

                var newProject = new Project();
                AddProjectWindowViewModel modalWindow = new AddProjectWindowViewModel(newProject, employees);
                if (await _uiVisualizerService.ShowDialogAsync(modalWindow) == true)
                {
                    var projectRep = uow.GetRepository<IProjectRepository>();
                    projectRep.Add(newProject);

                    if (newProject.Leader != null)
                        employeeRep.Update(newProject.Leader);
                    if (newProject.Employees.Count != 0)
                    {
                        foreach (var employee in newProject.Employees)
                        {
                            employeeRep.Update(employee);
                        }
                    }
                    uow.SaveChanges();
                    Projects = CollectionViewSource.GetDefaultView(projectRep.GetAll());
                    Employees = CollectionViewSource.GetDefaultView(employeeRep.GetAll());
                }
            }
        }

        #endregion

        #region Change command

        private TaskCommand _changeCommand;

        /// <summary>
        /// Gets the Change command.
        /// </summary>
        public TaskCommand ChangeCommand
        {
            get { return _changeCommand ?? (_changeCommand = new TaskCommand(Change, CanChange)); }
        }

        /// <summary>
        /// Method to invoke when the Change command is executed.
        /// </summary>
        private async Task Change()
        {
            if (SelectedTabIndex == 0)
            {
                await ChangeProject();
            }
            else
            {
                await ChangeEmployee();
            }
            
        }

        /// <summary>
        /// Method to check whether the Change command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanChange()
        {
            if (SelectedTabIndex == 0)
            {
                return SelectedProject != null;
            }
            else
            {
                return SelectedEmployee != null;
            }
        }

        private async Task ChangeProject()
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var employeeRep = uow.GetRepository<IEmployeeRepository>();
                var employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                var projectRep = uow.GetRepository<IProjectRepository>();

                
                var previsoLeader = SelectedProject.Leader;
                var typeFactory = this.GetTypeFactory();
                List<Employee> chengedEmpl = new List<Employee>();
                var modalWindow =
                    typeFactory.CreateInstanceWithParametersAndAutoCompletion<ChangeProjectViewModel>(SelectedProject,
                        employees, chengedEmpl);
                
                if (await _uiVisualizerService.ShowDialogAsync(modalWindow) == true)
                {
                    
                    projectRep.Update(SelectedProject);
                    

                    if (SelectedProject.Leader != null)
                        employeeRep.Update(SelectedProject.Leader);
                    else
                    {
                        if (previsoLeader != null)
                        {
                            previsoLeader.LeaderToProjects.Remove(SelectedProject);
                            previsoLeader.Projects.Remove(SelectedProject);
                            employeeRep.Update(previsoLeader);
                        }
                    }
                        
                    if (SelectedProject.Employees.Count != 0)
                    {
                        foreach (var employee in SelectedProject.Employees)
                        {
                            employeeRep.Update(employee);
                        }
                    }
                    if (chengedEmpl.Count > 0)
                    {
                        foreach (var employee in chengedEmpl)
                        {
                            employeeRep.Update(employee);
                        }
                    }
                    
                    uow.SaveChanges();
                    
                    Projects = CollectionViewSource.GetDefaultView(projectRep.GetAll());
                    Employees = CollectionViewSource.GetDefaultView(employeeRep.GetAll());
                }
            }
        }
        private async Task ChangeEmployee()
        {
            
        }

        #endregion

        #region Employees property

        /// <summary>
        /// Gets or sets the Employees value.
        /// </summary>
        public ICollectionView Employees
        {
            get { return GetValue<ICollectionView>(EmployeesProperty); }
            set { SetValue(EmployeesProperty, value); }
        }

        /// <summary>
        /// Employees property data.
        /// </summary>
        public static readonly PropertyData EmployeesProperty = RegisterProperty("Employees", typeof(ICollectionView));

        #endregion

        #region SelectedEmployee property

        /// <summary>
        /// Gets or sets the SelectedEmployee value.
        /// </summary>
        public Employee SelectedEmployee
        {
            get { return GetValue<Employee>(SelectedEmployeeProperty); }
            set { SetValue(SelectedEmployeeProperty, value); }
        }

        /// <summary>
        /// SelectedEmployee property data.
        /// </summary>
        public static readonly PropertyData SelectedEmployeeProperty = RegisterProperty("SelectedEmployee", typeof(Employee));

        #endregion

        #region Projects property

        /// <summary>
        /// Gets or sets the Projects value.
        /// </summary>
        public ICollectionView Projects
        {
            get { return GetValue<ICollectionView>(ProjectsProperty); }
            set { SetValue(ProjectsProperty, value); }
        }

        /// <summary>
        /// Projects property data.
        /// </summary>
        public static readonly PropertyData ProjectsProperty = RegisterProperty("Projects", typeof(ICollectionView));

        #endregion

        #region SelectedProject property

        /// <summary>
        /// Gets or sets the SelectedProject value.
        /// </summary>
        
        public Project SelectedProject
        {
            get { return GetValue<Project>(SelectedProjectProperty); }
            set { SetValue(SelectedProjectProperty, value); }
        }

        /// <summary>
        /// SelectedProject property data.
        /// </summary>
        public static readonly PropertyData SelectedProjectProperty = RegisterProperty("SelectedProject", typeof(Project));

        #endregion

        #region SelectedTabIndex property

        /// <summary>
        /// Gets or sets the SelectedTabIndex value.
        /// </summary>
        public int SelectedTabIndex
        {
            get { return GetValue<int>(SelectedTabIndexProperty); }
            set { SetValue(SelectedTabIndexProperty, value); }
        }

        /// <summary>
        /// SelectedTabIndex property data.
        /// </summary>
        public static readonly PropertyData SelectedTabIndexProperty = RegisterProperty("SelectedTabIndex", typeof(int));

        #endregion

     
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();
           
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
