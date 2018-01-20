using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Catel.Data;

namespace Demo.Models.DAL
{
    public class Project : ValidatableModelBase
    {

        #region Id property

        /// <summary>
        /// Gets or sets the Id value.
        /// </summary>
        public int Id
        {
            get { return GetValue<int>(IdProperty); }
            set { SetValue(IdProperty, value); }
        }

        /// <summary>
        /// Id property data.
        /// </summary>
        public static readonly PropertyData IdProperty = RegisterProperty("Id", typeof(int));

        #endregion

        #region Customer property

        /// <summary>
        /// Gets or sets the Customer value.
        /// </summary>
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

        #region Leader property

        /// <summary>
        /// Gets or sets the Leader value.
        /// </summary>
        public virtual Employee Leader
        {
            get { return GetValue<Employee>(LeaderProperty); }
            set { SetValue(LeaderProperty, value); }
        }

        /// <summary>
        /// Leader property data.
        /// </summary>
        public static readonly PropertyData LeaderProperty = RegisterProperty("Leader", typeof(Employee));

        #endregion

        #region Priority property

        /// <summary>
        /// Gets or sets the Priority value.
        /// </summary>
        public int Priority
        {
            get { return GetValue<int>(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        /// <summary>
        /// Priority property data.
        /// </summary>
        public static readonly PropertyData PriorityProperty = RegisterProperty("Priority", typeof(int),1);

        #endregion

        #region DateTimeStart property

        /// <summary>
        /// Gets or sets the DateTimeStart value.
        /// </summary>
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

        #region Employees property

        /// <summary>
        /// Gets or sets the Employees value.
        /// </summary>
        public virtual ObservableCollection<Employee> Employees
        {
            get { return GetValue<ObservableCollection<Employee>>(EmployeesProperty); }
            set { SetValue(EmployeesProperty, value); }
        }

        /// <summary>
        /// Employees property data.
        /// </summary>
        public static readonly PropertyData EmployeesProperty = RegisterProperty("Employees", typeof(ObservableCollection<Employee>));

        #endregion
        public Project()
        {
            Employees = new ObservableCollection<Employee>();
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(Customer))
            {
                validationResults.Add(FieldValidationResult.CreateError(CustomerProperty,"Необходимо имя заказчика"));
            }
            if (string.IsNullOrWhiteSpace(Performer))
            {
                validationResults.Add(FieldValidationResult.CreateError(PerformerProperty,"Необходимо имя исполнителя"));
            }
        }
    }
}