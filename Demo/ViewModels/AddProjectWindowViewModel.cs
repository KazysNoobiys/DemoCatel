using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Catel.Collections;
using Catel.Data;
using Demo.Models.DAL;
using Orc.EntityFramework;

namespace Demo.ViewModels
{
    using Catel.MVVM;
    using System.Threading.Tasks;

    public class AddProjectWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Employee> _employees;
        public override string Title { get { return "Новый проект"; } }
        public AddProjectWindowViewModel(Project project, ObservableCollection<Employee> employees)
        {
            Project = project;
            _employees = employees;
            DateTimeEnd = DateTime.Now.Date;
            DateTimeStart = DateTime.Now.Date;

        }

        #region Property


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
        [ViewModelToModel("Project")]
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
        [ViewModelToModel("Project")]
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
        [ViewModelToModel("Project")]
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

        #region Leader property

        /// <summary>
        /// Gets or sets the Leader value.
        /// </summary>

        public virtual IncludeEmployee Leader
        {
            get { return GetValue<IncludeEmployee>(LeaderProperty); }
            set { SetValue(LeaderProperty, value); }
        }

        /// <summary>
        /// Leader property data.
        /// </summary>
        public static readonly PropertyData LeaderProperty = RegisterProperty("Leader", typeof(IncludeEmployee), null, LeaderChangedHandler);

        private static void LeaderChangedHandler(object sender, AdvancedPropertyChangedEventArgs advancedPropertyChangedEventArgs)
        {

            var vm = sender as AddProjectWindowViewModel;
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
            var q = from e in listEmployees
                    where e.Employee == vm.Leader.Employee
                    select e;
            listEmployees.Remove(q.First());

        }

        #endregion

        #region ListLeaders property

        /// <summary>
        /// Gets or sets the ListLeaders value.
        /// </summary>
        public ObservableCollection<IncludeEmployee> ListLeaders
        {
            get { return GetValue<ObservableCollection<IncludeEmployee>>(ListLeadersProperty); }
            set { SetValue(ListLeadersProperty, value); }
        }

        /// <summary>
        /// ListLeaders property data.
        /// </summary>
        public static readonly PropertyData ListLeadersProperty = RegisterProperty("ListLeaders", typeof(ObservableCollection<IncludeEmployee>));

        #endregion

        #region DateTimeStart property

        /// <summary>
        /// Gets or sets the DateTimeStart value.
        /// </summary>
        [ViewModelToModel("Project")]
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
        [ViewModelToModel("Project")]
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
        [ViewModelToModel("Project")]
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

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ListLeaders = new ObservableCollection<IncludeEmployee>();
            IncludeEmployees = new ObservableCollection<IncludeEmployee>();
            foreach (var employee in _employees)
            {
                ListLeaders.Add(new IncludeEmployee(employee, false));
                IncludeEmployees.Add(new IncludeEmployee(employee, false));
            }
        }

        protected override Task<bool> SaveAsync()
        {
            if (Leader != null)
            {
                Leader.Employee.LeaderToProjects.Add(Project);
                Leader.Employee.Projects.Add(Project);
            }
            foreach (var employee in IncludeEmployees)
            {
                if (employee.IsInclude)
                {
                    employee.Employee.Projects.Add(Project);
                    Project.Employees.Add(employee.Employee);
                }
            }
            return base.SaveAsync();
        }
     
    }
}
