using System;
using System.Collections.Generic;
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

    public class ChangeProjectViewModel : ViewModelBase
    {
        private readonly ObservableCollection<Employee> _employees;
        private List<Employee> _chengedEmpl;
        public ChangeProjectViewModel(Project project, ObservableCollection<Employee> employees,  List<Employee> chengedEmpl)
        {
            _employees = employees;
            Project = project;
            _chengedEmpl = chengedEmpl;
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
                SelectedLeader = null;
                SelectedIndex = -1;
            }
            else
            {
                EnabledListLeaders = true;
            }
        }

        #endregion

        public override string Title { get { return "View model title"; } }

        #region Project property

        /// <summary>
        /// Gets or sets the Project value.
        /// </summary>
        [Model]
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
        public ObservableCollection<IncludeEmployee> ListLeaders
        {
            get { return GetValue<ObservableCollection<IncludeEmployee>>(ListLeadersProperty); }
            set { SetValue(ListLeadersProperty, value); }
        }

        /// <summary>
        /// ListLeader property data.
        /// </summary>
        public static readonly PropertyData ListLeadersProperty = RegisterProperty("ListLeaders", typeof(ObservableCollection<IncludeEmployee>));

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
        public static readonly PropertyData SelectedLeaderProperty = RegisterProperty("SelectedLeader", typeof(IncludeEmployee),null,SelectedLeaderChange);

        private static void SelectedLeaderChange(object sender, AdvancedPropertyChangedEventArgs advancedPropertyChangedEventArgs)
        {
            var vm = sender as ChangeProjectViewModel;
            var listEmployees = vm.IncludeEmployees;

            foreach (var employee in vm._employees)
            {
                bool find = false;
                foreach (var listEmployee in listEmployees)
                {
                    if (employee == listEmployee.Employee)
                    {
                        find = true;
                        break;
                    }
                }
                if (!find)
                    listEmployees.Add(new IncludeEmployee(employee, true));
            }
            if (vm.SelectedLeader != null)
            {
                var q = from e in listEmployees
                    where e.Employee == vm.SelectedLeader.Employee
                    select e;
                listEmployees.Remove(q.First());
            }
            

        }

        #endregion

        #region IncludeEmployees property

        /// <summary>
        /// Gets or sets the IncludeEmployees value.
        /// </summary>
        public ObservableCollection<IncludeEmployee> IncludeEmployees
        {
            get { return GetValue<ObservableCollection<IncludeEmployee>>(IncludeEmployeesProperty); }
            set { SetValue(IncludeEmployeesProperty, value); }
        }

        /// <summary>
        /// IncludeEmployees property data.
        /// </summary>
        public static readonly PropertyData IncludeEmployeesProperty = RegisterProperty("IncludeEmployees", typeof(ObservableCollection<IncludeEmployee>));

        #endregion

        #region SelectedIndex property

        /// <summary>
        /// Gets or sets the SelectedIndex value.
        /// </summary>
        public int SelectedIndex
        {
            get { return GetValue<int>(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// SelectedIndex property data.
        /// </summary>
        public static readonly PropertyData SelectedIndexProperty = RegisterProperty("SelectedIndex", typeof(int));

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
        public static readonly PropertyData EnabledListLeadersProperty = RegisterProperty("EnabledListLeaders", typeof(bool),true);

        #endregion
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ListLeaders = new ObservableCollection<IncludeEmployee>();
            IncludeEmployees = new ObservableCollection<IncludeEmployee>();
            foreach (var employee in _employees)
            {
                ListLeaders.Add(new IncludeEmployee(employee, false));

                if (Project.Employees != null)
                {
                    foreach (var projectEmployee in Project.Employees)
                    {
                        if (employee.Equals(projectEmployee))
                        {
                            IncludeEmployees.Add(new IncludeEmployee(employee, true));
                        }
                    }
                }
                
                IncludeEmployees.Add(new IncludeEmployee(employee, false));
            }

            if (Project.Leader != null)
            {
                int i = 0;
                foreach (var leader in ListLeaders)
                {
                    if (leader.Employee == Project.Leader)
                    {
                        SelectedLeader = leader;
                        SelectedIndex = i;
                        IncludeEmployees.Remove(SelectedLeader);
                    }
                    i++;
                }
            }
           
                
        }

        protected override async Task CloseAsync()
        {
            foreach (var includeEmployee in IncludeEmployees)
            {
                if (includeEmployee.IsInclude)
                {
                    if (!Project.Employees.Contains(includeEmployee.Employee))
                    {
                        Project.Employees.Add(includeEmployee.Employee);
                        includeEmployee.Employee.Projects.Add(Project);
                    }
                }
                else
                {
                    if (Project.Employees.Contains(includeEmployee.Employee))
                    {
                        Project.Employees.Remove(includeEmployee.Employee);
                        includeEmployee.Employee.Projects.Remove(Project);
                        _chengedEmpl.Add(includeEmployee.Employee);
                    }
                }
            }

            if (SelectedLeader == null)
            {
                if (Project.Leader != null)
                {
                    Project.Leader.LeaderToProjects.Remove(Project);
                    Project.Leader.Projects.Remove(Project);
                    Project.Leader = null;
                }
            }
            else
            {
                Project.Leader = SelectedLeader.Employee;
                Project.Leader.LeaderToProjects.Add(Project);
                Project.Leader.Projects.Add(Project);
            }
            await base.CloseAsync();
        }
    }
}
