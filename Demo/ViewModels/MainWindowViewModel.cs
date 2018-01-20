using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;
using System.Windows.Documents;
using Catel.Data;
using Catel.IoC;
using Catel.Services;
using Demo.Models.DAL;
using Demo.Models.Repository;
using LinqKit;
using Orc.EntityFramework;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;
    using Models;

    static class CollectionExtensions
    {
        public static bool FindContains(this ICollection<Project> projects, string str)
        {
            bool find = false;
            foreach (var project in projects)
            {
                if (project.Customer.ToLower().Contains(str) || project.Performer.ToLower().Contains(str))
                {
                    find = true;
                    break;
                }
            }
            return find;
        }
    }

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService _uiVisualizerService;
        private readonly IMessageService _messageService;
        private ObservableCollection<Project> _projects;
        private ObservableCollection<Employee> _employees;

        public override string Title { get { return "DemoCatel"; } }

        public MainWindowViewModel(IUIVisualizerService uiVisualizerService, IMessageService messageService)
        {
            _uiVisualizerService = uiVisualizerService;
            _messageService = messageService;
           
        }

        #region Filtering

        #region FilteringProjects

        private ObservableCollection<Project> FilteringProjects(ObservableCollection<Project> projects)
        {
            var predicate = PredicateBuilder.New<Project>();
            if (!string.IsNullOrWhiteSpace(FilterOnCustomer))
            {
                predicate.And(p => p.Customer.ToLower().Contains(FilterOnCustomer.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(FilterOnPerformer))
            {
                predicate.And(p => p.Performer.ToLower().Contains(FilterOnPerformer.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(FilterOnLeaderLastName))
            {
                predicate.And(p => p.Leader != null && p.Leader.LastName.ToLower()
                                       .Contains(FilterOnLeaderLastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(FilterOnLeaderFirstName))
            {
                predicate.And(p => p.Leader != null && p.Leader.LastName.ToLower()
                                       .Contains(FilterOnLeaderFirstName.ToLower()));
            }
            if (DateTimeStartCheckBox)
            {
                predicate.And(p => p.DateTimeStart == FilterOnDateTimeStart);
            }
            if (DateTimeEndCheckBox)
            {
                predicate.And(p => p.DateTimeEnd == FilterOnDateTimeEnd);
            }

            var listProjects = projects.Where(predicate).ToList();

            if (listProjects.Count != 0)
            {
                return new ObservableCollection<Project>(listProjects);
            }
            else
            {
                return null;
            }


        }
        private static void FilterProjectChanged(object sender, AdvancedPropertyChangedEventArgs advancedPropertyChangedEventArgs)
        {
            var mw = sender as MainWindowViewModel;

            if (string.IsNullOrWhiteSpace(mw.FilterOnCustomer)
                && string.IsNullOrWhiteSpace(mw.FilterOnPerformer)
                && string.IsNullOrWhiteSpace(mw.FilterOnLeaderLastName)
                && string.IsNullOrWhiteSpace(mw.FilterOnLeaderFirstName)
                && !mw.DateTimeStartCheckBox
                && !mw.DateTimeEndCheckBox)
            {
                mw.Projects = mw._projects;
            }
            else
            {
                ObservableCollection<Project> list;
                list = mw.FilteringProjects(mw._projects);
                if (list != null)
                    mw.Projects = list;
                else
                {
                    mw.Projects = new ObservableCollection<Project>(new List<Project>());
                }
            }

        }

        #region FilterOnCustomer property

        /// <summary>
        /// Gets or sets the FilterOnCustomer value.
        /// </summary>
        public string FilterOnCustomer
        {
            get { return GetValue<string>(FilterOnCustomerProperty); }
            set { SetValue(FilterOnCustomerProperty, value); }
        }

        /// <summary>
        /// FilterOnCustomer property data.
        /// </summary>
        public static readonly PropertyData FilterOnCustomerProperty = RegisterProperty("FilterOnCustomer",
            typeof(string), null, FilterProjectChanged);

        #endregion

        #region FilterOnPerformer property

        /// <summary>
        /// Gets or sets the FilterOnPerformer value.
        /// </summary>
        public string FilterOnPerformer
        {
            get { return GetValue<string>(FilterOnPerformerProperty); }
            set { SetValue(FilterOnPerformerProperty, value); }
        }

        /// <summary>
        /// FilterOnPerformer property data.
        /// </summary>
        public static readonly PropertyData FilterOnPerformerProperty = RegisterProperty("FilterOnPerformer",
            typeof(string), null, FilterProjectChanged);

        #endregion

        #region FilterOnLeaderFirstName property

        /// <summary>
        /// Gets or sets the FilterOnLeaderFirstName value.
        /// </summary>
        public string FilterOnLeaderFirstName
        {
            get { return GetValue<string>(FilterOnLeaderFirstNameProperty); }
            set { SetValue(FilterOnLeaderFirstNameProperty, value); }
        }

        /// <summary>
        /// FilterOnLeaderFirstName property data.
        /// </summary>
        public static readonly PropertyData FilterOnLeaderFirstNameProperty =
            RegisterProperty("FilterOnLeaderFirstName", typeof(string), null, FilterProjectChanged);

        #endregion

        #region FilterOnLeaderLastName property

        /// <summary>
        /// Gets or sets the FilterOnLeaderLastName value.
        /// </summary>
        public string FilterOnLeaderLastName
        {
            get { return GetValue<string>(FilterOnLeaderLastNameProperty); }
            set { SetValue(FilterOnLeaderLastNameProperty, value); }
        }

        /// <summary>
        /// FilterOnLeaderLastName property data.
        /// </summary>
        public static readonly PropertyData FilterOnLeaderLastNameProperty = RegisterProperty("FilterOnLeaderLastName",
            typeof(string), null, FilterProjectChanged);

        #endregion

        #region FilterOnDateTimeStart property

        /// <summary>
        /// Gets or sets the FiltertOnDateTimeStart value.
        /// </summary>
        public DateTime FilterOnDateTimeStart
        {
            get { return GetValue<DateTime>(FilterOnDateTimeStartProperty); }
            set { SetValue(FilterOnDateTimeStartProperty, value); }
        }

        /// <summary>
        /// FiltertOnDateTimeStart property data.
        /// </summary>
        public static readonly PropertyData FilterOnDateTimeStartProperty = RegisterProperty("FilterOnDateTimeStart",
            typeof(DateTime), default(DateTime), FilterProjectChanged);

        #endregion

        #region FilterOnDateTimeEnd property

        /// <summary>
        /// Gets or sets the FilterOnDateTimeEnd value.
        /// </summary>
        public DateTime FilterOnDateTimeEnd
        {
            get { return GetValue<DateTime>(FilterOnDateTimeEndProperty); }
            set { SetValue(FilterOnDateTimeEndProperty, value); }
        }

        /// <summary>
        /// FilterOnDateTimeEnd property data.
        /// </summary>
        public static readonly PropertyData FilterOnDateTimeEndProperty = RegisterProperty("FilterOnDateTimeEnd",
            typeof(DateTime), default(DateTime), FilterProjectChanged);

        #endregion

        #region DateTimeStartCheckBox property

        /// <summary>
        /// Gets or sets the DateTimeStartCheckBox value.
        /// </summary>
        public bool DateTimeStartCheckBox
        {
            get { return GetValue<bool>(DateTimeStartCheckBoxProperty); }
            set { SetValue(DateTimeStartCheckBoxProperty, value); }
        }

        /// <summary>
        /// DateTimeStartCheckBox property data.
        /// </summary>
        public static readonly PropertyData DateTimeStartCheckBoxProperty = RegisterProperty("DateTimeStartCheckBox",
            typeof(bool), false, FilterProjectChanged);

        #endregion

        #region DateTimeEndCheckBox property

        /// <summary>
        /// Gets or sets the DateTimeEndCheckBox value.
        /// </summary>
        public bool DateTimeEndCheckBox
        {
            get { return GetValue<bool>(DateTimeEndCheckBoxProperty); }
            set { SetValue(DateTimeEndCheckBoxProperty, value); }
        }

        /// <summary>
        /// DateTimeEndCheckBox property data.
        /// </summary>
        public static readonly PropertyData DateTimeEndCheckBoxProperty = RegisterProperty("DateTimeEndCheckBox",
            typeof(bool), false, FilterProjectChanged);

        #endregion

        #endregion

        #region FilteringEmployees

        private ObservableCollection<Employee> FilteringEmployees(ObservableCollection<Employee> employees)
        {
            var predicate = PredicateBuilder.New<Employee>();
            if (!string.IsNullOrWhiteSpace(FilterOnEmpFirstName))
            {
                predicate.And(e => e.FirstName.ToLower().Contains(FilterOnEmpFirstName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(FilterOnEmpLastName))
            {
                predicate.And(e => e.LastName.ToLower().Contains(FilterOnEmpLastName.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(FilterOnEmpProjects))
            {
                predicate.And(e => e.Projects != null && e.Projects.FindContains(FilterOnEmpProjects));
            }


            var listEmployees = employees.Where(predicate).ToList();

            if (listEmployees.Count != 0)
            {
                return new ObservableCollection<Employee>(listEmployees);
            }
            else
            {
                return null;
            }
        }
        private static void FilterEmploeeChanged(object sender, AdvancedPropertyChangedEventArgs advancedPropertyChangedEventArgs)
        {
            var mw = sender as MainWindowViewModel;

            if (string.IsNullOrWhiteSpace(mw.FilterOnEmpFirstName)
                && string.IsNullOrWhiteSpace(mw.FilterOnEmpLastName)
                && string.IsNullOrWhiteSpace(mw.FilterOnEmpProjects))
            {
                mw.Employees = mw._employees;
            }
            else
            {
                ObservableCollection<Employee> list;
                list = mw.FilteringEmployees(mw._employees);
                if (list != null)
                    mw.Employees = list;
                else
                {
                    mw.Employees = new ObservableCollection<Employee>(new List<Employee>());
                }

            }

        }

        #region FilterOnEmpFirstName property

        /// <summary>
        /// Gets or sets the FilterOnEmpFirstName value.
        /// </summary>
        public string FilterOnEmpFirstName
        {
            get { return GetValue<string>(FilterOnEmpFirstNameProperty); }
            set { SetValue(FilterOnEmpFirstNameProperty, value); }
        }

        /// <summary>
        /// FilterOnEmpFirstName property data.
        /// </summary>
        public static readonly PropertyData FilterOnEmpFirstNameProperty = RegisterProperty("FilterOnEmpFirstName",
            typeof(string), null, FilterEmploeeChanged);


        #endregion

        #region FilterOnEmpLastName property

        /// <summary>
        /// Gets or sets the FilterOnEmpLastName value.
        /// </summary>
        public string FilterOnEmpLastName
        {
            get { return GetValue<string>(FilterOnEmpLastNameProperty); }
            set { SetValue(FilterOnEmpLastNameProperty, value); }
        }

        /// <summary>
        /// FilterOnEmpLastName property data.
        /// </summary>
        public static readonly PropertyData FilterOnEmpLastNameProperty = RegisterProperty("FilterOnEmpLastName", 
            typeof(string),null, FilterEmploeeChanged);

        #endregion

        #region FilterOnEmpProjects property

        /// <summary>
        /// Gets or sets the FilterOnEmpProjects value.
        /// </summary>
        public string FilterOnEmpProjects
        {
            get { return GetValue<string>(FilterOnEmpProjectsProperty); }
            set { SetValue(FilterOnEmpProjectsProperty, value); }
        }

        /// <summary>
        /// FilterOnEmpProjects property data.
        /// </summary>
        public static readonly PropertyData FilterOnEmpProjectsProperty = RegisterProperty("FilterOnEmpProjects", 
            typeof(string),null, FilterEmploeeChanged);

        #endregion

        #endregion

        #endregion

        #region Commands

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

                    
                   
                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterEmploeeChanged(this,null);
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

                    

                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterProjectChanged(this, null);
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
                var typeFactory = this.GetTypeFactory();

                var modalWindow =
                    typeFactory.CreateInstanceWithParametersAndAutoCompletion<ChangeProjectViewModel>(SelectedProject.Id);

                if (await _uiVisualizerService.ShowDialogAsync(modalWindow) == true)
                {
                    var employeeRep = uow.GetRepository<IEmployeeRepository>();
                    var projectRep = uow.GetRepository<IProjectRepository>();

                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterProjectChanged(this,null);
                }
            }
        }
        private async Task ChangeEmployee()
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var typeFactory = this.GetTypeFactory();

                var modalWindow =
                    typeFactory.CreateInstanceWithParametersAndAutoCompletion<ChangeEmployeeViewModel>(SelectedEmployee.Id);

                if (await _uiVisualizerService.ShowDialogAsync(modalWindow) == true)
                {
                    var employeeRep = uow.GetRepository<IEmployeeRepository>();
                    var projectRep = uow.GetRepository<IProjectRepository>();

                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterEmploeeChanged(this,null);
                }
            }
        }

        #endregion

        #region Delete command

        private TaskCommand _deleteCommand;

        /// <summary>
        /// Gets the Delete command.
        /// </summary>
        public TaskCommand DeleteCommand
        {
            get { return _deleteCommand ?? (_deleteCommand = new TaskCommand(Delete, CanDelete)); }
        }

        /// <summary>
        /// Method to invoke when the Delete command is executed.
        /// </summary>
        private async Task Delete()
        {
            if (SelectedTabIndex == 0)
            {
                await DeleteProject();
            }
            else
            {
                await DeleteEmployee();
            }
        }

        private async Task DeleteEmployee()
        {
            if (await _messageService.ShowAsync($"Вы уверены что хотите удалить сотрудника c Id = {SelectedEmployee.Id}?",
                    "Внимание! Удаление сотрудника", MessageButton.YesNo) == MessageResult.Yes)
            {
                using (var uow = new UnitOfWork<ProjectDbContext>())
                {
                    var employeeRep = uow.GetRepository<IEmployeeRepository>();
                    var projectRep = uow.GetRepository<IProjectRepository>();

                    employeeRep.Attach(SelectedEmployee);
                    employeeRep.Delete(SelectedEmployee);

                    uow.SaveChanges();

                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterEmploeeChanged(this,null);
                }
            }

        }

        private async Task DeleteProject()
        {
            if (await _messageService.ShowAsync($"Вы уверены что хотите удалить проект c Id = {SelectedProject.Id}?",
                    "Внимание! Удаление проекта", MessageButton.YesNo) == MessageResult.Yes)
            {
                using (var uow = new UnitOfWork<ProjectDbContext>())
                {
                    var employeeRep = uow.GetRepository<IEmployeeRepository>();
                    var projectRep = uow.GetRepository<IProjectRepository>();

                    projectRep.Attach(SelectedProject);
                    projectRep.Delete(SelectedProject);

                    uow.SaveChanges();

                    _projects = new ObservableCollection<Project>(projectRep.GetAll());
                    Projects = _projects;
                    _employees = new ObservableCollection<Employee>(employeeRep.GetAll());
                    Employees = _employees;

                    FilterProjectChanged(this,null);
                }
            }
        }

        /// <summary>
        /// Method to check whether the Delete command can be executed.
        /// </summary>
        /// <returns><c>true</c> if the command can be executed; otherwise <c>false</c></returns>
        private bool CanDelete()
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

        #endregion

        #region HideDetails command

        private Command _hideDetailsCommand;

        /// <summary>
        /// Gets the HideDetails command.
        /// </summary>
        public Command HideDetailsCommand
        {
            get { return _hideDetailsCommand ?? (_hideDetailsCommand = new Command(HideDetails)); }
        }

        /// <summary>
        /// Method to invoke when the HideDetails command is executed.
        /// </summary>
        private void HideDetails()
        {
            if (SelectedTabIndex == 0)
                SelectedProjectIndex = -1;
            else
                SelectedEmployeeIndex = -1;

        }

        #endregion

        #region ResetFilterProjects command

        private Command _resetFilterProjectsCommand;

        /// <summary>
        /// Gets the ResetFilterProjects command.
        /// </summary>
        public Command ResetFilterProjectsCommand
        {
            get { return _resetFilterProjectsCommand ?? (_resetFilterProjectsCommand = new Command(ResetFilterProjects)); }
        }

        /// <summary>
        /// Method to invoke when the ResetFilterProjects command is executed.
        /// </summary>
        private void ResetFilterProjects()
        {
            FilterOnCustomer = FilterOnPerformer = FilterOnLeaderFirstName =
                FilterOnLeaderLastName = string.Empty;
            DateTimeEndCheckBox = DateTimeStartCheckBox = false;
        }

        #endregion

        #region ResetFilterEmployees command

        private Command _resetFilterEmployeesCommand;

        /// <summary>
        /// Gets the ResetFilterEmployees command.
        /// </summary>
        public Command ResetFilterEmployeesCommand
        {
            get { return _resetFilterEmployeesCommand ?? (_resetFilterEmployeesCommand = new Command(ResetFilterEmployees)); }
        }

        /// <summary>
        /// Method to invoke when the ResetFilterEmployees command is executed.
        /// </summary>
        private void ResetFilterEmployees()
        {
            FilterOnEmpFirstName = FilterOnEmpLastName = FilterOnEmpProjects = string.Empty;
        }

        #endregion

        #endregion

        #region Properties

        #region Employees property

        /// <summary>
        /// Gets or sets the Employees value.
        /// </summary>
        public ObservableCollection<Employee> Employees
        {
            get { return GetValue<ObservableCollection<Employee>>(EmployeesProperty); }
            set { SetValue(EmployeesProperty, value); }
        }

        /// <summary>
        /// Employees property data.
        /// </summary>
        public static readonly PropertyData EmployeesProperty = RegisterProperty("Employees", 
            typeof(ObservableCollection<Employee>));

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
        public ObservableCollection<Project> Projects
        {
            get { return GetValue<ObservableCollection<Project>>(ProjectsProperty); }
            set { SetValue(ProjectsProperty, value); }
        }

        /// <summary>
        /// Projects property data.
        /// </summary>
        public static readonly PropertyData ProjectsProperty = RegisterProperty("Projects", 
            typeof(ObservableCollection<Project>));

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

        #region SelectedProjectIndex property

        /// <summary>
        /// Gets or sets the SelectedProjectIndex value.
        /// </summary>
        public int SelectedProjectIndex
        {
            get { return GetValue<int>(SelectedProjectIndexProperty); }
            set { SetValue(SelectedProjectIndexProperty, value); }
        }

        /// <summary>
        /// SelectedProjectIndex property data.
        /// </summary>
        public static readonly PropertyData SelectedProjectIndexProperty = RegisterProperty("SelectedProjectIndex",
            typeof(int), -1);

        #endregion

        #region SelectedEmployeeIndex property

        /// <summary>
        /// Gets or sets the SelectedEmployeeIndex value.
        /// </summary>
        public int SelectedEmployeeIndex
        {
            get { return GetValue<int>(SelectedEmployeeIndexProperty); }
            set { SetValue(SelectedEmployeeIndexProperty, value); }
        }

        /// <summary>
        /// SelectedEmployeeIndex property data.
        /// </summary>
        public static readonly PropertyData SelectedEmployeeIndexProperty = RegisterProperty("SelectedEmployeeIndex", 
            typeof(int),-1);

        #endregion

        #endregion


        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var projectRep = uow.GetRepository<IProjectRepository>();
                var employeeRep = uow.GetRepository<IEmployeeRepository>();


                _projects = new ObservableCollection<Project>(projectRep.GetAll().Include("Employees"));
                Projects = _projects;

                _employees = new ObservableCollection<Employee>(employeeRep.GetAll()
                    .Include("Projects").Include("LeaderToProjects"));
                Employees = _employees;

                FilterOnDateTimeStart = DateTime.MaxValue;
                FilterOnDateTimeEnd = DateTime.MaxValue;

            }
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
