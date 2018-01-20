using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Data;
using Catel.Data;
using Catel.Runtime.Serialization;
using Demo.Models.DAL;
using Demo.Models.Repository;
using Orc.EntityFramework;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class ChangeProjectViewModel : ViewModelBase
    {

        private readonly ObservableCollection<Employee> _employees;
        public override string Title { get { return "Изменение проекта"; } }

        public ChangeProjectViewModel(int projectId)
        {
            using (var uow = new UnitOfWork<ProjectDbContext>())
            {
                var employeeRep = uow.GetRepository<IEmployeeRepository>();
                var projectRep = uow.GetRepository<IProjectRepository>();

                _employees = new ObservableCollection<Employee>(employeeRep.GetAll());

                Project = projectRep.Find(p => p.Id == projectId).First();
            }
        }

        #region OffLeader command

        private Command _offLeaderCommand;

        /// <summary>
        /// Gets the OffLeader command.
        /// </summary>
        public Command OffLeaderCommand
        {
            get { return _offLeaderCommand ?? (_offLeaderCommand = new Command(OffLeader)); }
        }

        /// <summary>
        /// Method to invoke when the OffLeader command is executed.
        /// </summary>
        private void OffLeader()
        {
            if (EnabledListLeaders)
            {
                EnabledListLeaders = false;
            }
            else
            {
                EnabledListLeaders = true;
            }
        }

        #endregion

        #region Property

        #region Project property

        /// <summary>
        /// Gets or sets the Project value.
        /// </summary>
        [Model(SupportIEditableObject = false)]
        public Project Project
        {
            get { return GetValue<Project>(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        /// <summary>
        /// Project property data.
        /// </summary>
        public static readonly PropertyData ProjectProperty = RegisterProperty("Project", typeof(Project));

        #endregion

        #region Customer property

        /// <summary>
        /// Gets or sets the Customer value.
        /// </summary>
        [ViewModelToModel]
        public string Customer
        {
            get { return GetValue<string>(CustomerProperty); }
            set { SetValue(CustomerProperty, value); }
        }

        /// <summary>
        /// Customer property data.
        /// </summary>
        public static readonly PropertyData CustomerProperty = RegisterProperty("Customer", typeof(string));

        #endregion

        #region Performer property

        /// <summary>
        /// Gets or sets the Performer value.
        /// </summary>
        [ViewModelToModel]
        public string Performer
        {
            get { return GetValue<string>(PerformerProperty); }
            set { SetValue(PerformerProperty, value); }
        }

        /// <summary>
        /// Performer property data.
        /// </summary>
        public static readonly PropertyData PerformerProperty = RegisterProperty("Performer", typeof(string));

        #endregion

        #region Priority property

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        [ViewModelToModel]
        public int Priority
        {
            get { return GetValue<int>(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        /// <summary>
        /// Priority property data.
        /// </summary>
        public static readonly PropertyData PriorityProperty = RegisterProperty("Priority", typeof(int));

        #endregion

        #region DateTimeStart property

        /// <summary>
        /// Gets or sets the DateTimeStart value.
        /// </summary>
        [ViewModelToModel]
        public DateTime DateTimeStart
        {
            get { return GetValue<DateTime>(DateTimeStartProperty); }
            set { SetValue(DateTimeStartProperty, value); }
        }

        /// <summary>
        /// DateTimeStart property data.
        /// </summary>
        public static readonly PropertyData DateTimeStartProperty = RegisterProperty("DateTimeStart", typeof(DateTime));

        #endregion

        #region DateTimeEnd property

        /// <summary>
        /// Gets or sets the DateTimeEnd value.
        /// </summary>
        [ViewModelToModel]
        public DateTime DateTimeEnd
        {
            get { return GetValue<DateTime>(DateTimeEndProperty); }
            set { SetValue(DateTimeEndProperty, value); }
        }

        /// <summary>
        /// DateTimeEnd property data.
        /// </summary>
        public static readonly PropertyData DateTimeEndProperty = RegisterProperty("DateTimeEnd", typeof(DateTime));

        #endregion

        #region Description property

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        [ViewModelToModel]
        public string Description
        {
            get { return GetValue<string>(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Description property data.
        /// </summary>
        public static readonly PropertyData DescriptionProperty = RegisterProperty("Description", typeof(string));

        #endregion

        #region ListLeaders property

        /// <summary>
        /// Gets or sets the ListLeader value.
        /// </summary>
        public ICollectionView ListLeaders
        {
            get { return GetValue<ICollectionView>(ListLeadersProperty); }
            set { SetValue(ListLeadersProperty, value); }
        }

        /// <summary>
        /// ListLeader property data.
        /// </summary>
        public static readonly PropertyData ListLeadersProperty = RegisterProperty("ListLeaders", typeof(ICollectionView));

        #endregion

        #region SelectedLeader property

        /// <summary>
        /// Gets or sets the SelectedLeader value.
        /// </summary>
        public IncludeEmployee SelectedLeader
        {
            get { return GetValue<IncludeEmployee>(SelectedLeaderProperty); }
            set { SetValue(SelectedLeaderProperty, value); }
        }

        /// <summary>
        /// SelectedLeader property data.
        /// </summary>
        public static readonly PropertyData SelectedLeaderProperty = RegisterProperty("SelectedLeader",
            typeof(IncludeEmployee), null, SelectedLeaderChange);

        private static void SelectedLeaderChange(object sender, AdvancedPropertyChangedEventArgs advancedPropertyChangedEventArgs)
        {
            var vm = sender as ChangeProjectViewModel;
            var listEmployees = vm.IncludeEmployees;

            foreach (var employee in vm._employees)
            {
                bool find = false;
                foreach (IncludeEmployee listEmployee in listEmployees)
                {
                    if (employee == listEmployee.Employee)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    var source = listEmployees.SourceCollection as List<IncludeEmployee>;
                    source.Add(new IncludeEmployee(employee, false));
                    listEmployees.Refresh();
                }

            }
            if (vm.SelectedLeader != null)
            {
                var q = from e in listEmployees.SourceCollection as List<IncludeEmployee>
                    where e.Employee == vm.SelectedLeader.Employee
                    select e;

                var source = listEmployees.SourceCollection as List<IncludeEmployee>;
                source.Remove(q.First());
                listEmployees.Refresh();
            }


        }

        #endregion

        #region IncludeEmployees property

        /// <summary>
        /// Gets or sets the IncludeEmployees value.
        /// </summary>
        public ICollectionView IncludeEmployees
        {
            get { return GetValue<ICollectionView>(IncludeEmployeesProperty); }
            set { SetValue(IncludeEmployeesProperty, value); }
        }

        /// <summary>
        /// IncludeEmployees property data.
        /// </summary>
        public static readonly PropertyData IncludeEmployeesProperty = RegisterProperty("IncludeEmployees", typeof(ICollectionView));

        #endregion

        #region EnabledListLeaders property

        /// <summary>
        /// Gets or sets the EnabledListLeaders value.
        /// </summary>
        public bool EnabledListLeaders
        {
            get { return GetValue<bool>(EnabledListLeadersProperty); }
            set { SetValue(EnabledListLeadersProperty, value); }
        }

        /// <summary>
        /// EnabledListLeaders property data.
        /// </summary>
        public static readonly PropertyData EnabledListLeadersProperty = RegisterProperty("EnabledListLeaders", typeof(bool), true);

#endregion

        #endregion
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var listLeade = new List<IncludeEmployee>();
            var listEmp = new List<IncludeEmployee>();

            ListLeaders = new ListCollectionView(listLeade);
            IncludeEmployees = new ListCollectionView(listEmp);
            foreach (var employee in _employees)
            {
                listLeade.Add(new IncludeEmployee(employee, false));
                listEmp.Add(new IncludeEmployee(employee, false));
            }
            foreach (var projectEmployee in Project.Employees)
            {
                foreach (var includeEmployee in listEmp)
                {
                    if (projectEmployee == includeEmployee.Employee)
                        includeEmployee.IsInclude = true;
                }
            }

            if (Project.Leader != null)
            {
                foreach (var leader in listLeade)
                {
                    ListLeaders.MoveCurrentToNext();
                    if (leader.Employee == Project.Leader)
                    {
                        SelectedLeader = leader;
                        break;
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

                if (EnabledListLeaders)
                {
                    if (Project.Leader != null)
                    {
                        Project.Leader.LeaderToProjects.Remove(Project);
                        Project.Leader.Projects.Remove(Project);
                        employeeRep.Update(Project.Leader);

                        Project.Leader = SelectedLeader.Employee;
                        SelectedLeader.Employee.LeaderToProjects.Add(Project);
                        SelectedLeader.Employee.Projects.Add(Project);
                        employeeRep.Update(Project.Leader);
                        projectRep.Update(Project);
                    }
                    else
                    {
                        if (SelectedLeader != null)
                        {
                            Project.Leader = SelectedLeader.Employee;
                            SelectedLeader.Employee.LeaderToProjects.Add(Project);
                            SelectedLeader.Employee.Projects.Add(Project);
                            employeeRep.Update(Project.Leader);
                            projectRep.Update(Project);
                        }
                    }
                }
                else
                {
                    if (Project.Leader != null)
                    {
                        Project.Leader.LeaderToProjects.Remove(Project);
                        Project.Leader.Projects.Remove(Project);
                        employeeRep.Update(Project.Leader);

                        Project.Leader = null;
                        projectRep.Update(Project);
                    }
                }

                foreach (IncludeEmployee includeEmployee in IncludeEmployees)
                {
                    if (includeEmployee.IsInclude)
                    {
                        if (!Project.Employees.Contains(includeEmployee.Employee))
                        {
                            includeEmployee.Employee.Projects.Add(Project);
                            Project.Employees.Add(includeEmployee.Employee);
                            employeeRep.Update(includeEmployee.Employee);
                            projectRep.Update(Project);
                        }
                    }
                    if (!includeEmployee.IsInclude)
                    {
                        if (Project.Employees.Contains(includeEmployee.Employee))
                        {
                            includeEmployee.Employee.Projects.Remove(Project);
                            Project.Employees.Remove(includeEmployee.Employee);
                            employeeRep.Update(includeEmployee.Employee);
                            projectRep.Update(Project);
                        }
                    }
                }
                projectRep.Update(Project);
                uow.SaveChanges();
            }
            return base.SaveAsync();
        }

    }
}
