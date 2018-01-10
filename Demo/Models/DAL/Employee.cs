using System.Collections.Generic;
using Catel.Data;

namespace Demo.Models.DAL
{
    public class Employee:ValidatableModelBase
    {
        //public int Id { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Patronymic { get; set; }
        //public string Email { get; set; }
        //public virtual ICollection<Project> LeaderToProjects { get; set; }
        //public virtual ICollection<Project> Projects { get; set; }

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

        #region FirstName property

        /// <summary>
        /// Gets or sets the FirstName value.
        /// </summary>
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

        #region LeaderToProjects property

        /// <summary>
        /// Gets or sets the LeaderToProjects value.
        /// </summary>
        public virtual ICollection<Project> LeaderToProjects
        {
            get { return GetValue<ICollection<Project>>(LeaderToProjectsProperty); }
            set { SetValue(LeaderToProjectsProperty, value); }
        }

        /// <summary>
        /// LeaderToProjects property data.
        /// </summary>
        public static readonly PropertyData LeaderToProjectsProperty = RegisterProperty("LeaderToProjects", typeof(ICollection<Project>));

        #endregion

        #region Projects property

        /// <summary>
        /// Gets or sets the Projects value.
        /// </summary>
        public virtual ICollection<Project> Projects
        {
            get { return GetValue<ICollection<Project>>(ProjectsProperty); }
            set { SetValue(ProjectsProperty, value); }
        }

        /// <summary>
        /// Projects property data.
        /// </summary>
        public static readonly PropertyData ProjectsProperty = RegisterProperty("Projects", typeof(ICollection<Project>));

        #endregion
        public Employee()
        {
            Projects = new List<Project>();
            LeaderToProjects = new List<Project>();
        }

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                validationResults.Add(FieldValidationResult.CreateError(FirstNameProperty, "Необходимо имя сотрудника"));
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                validationResults.Add(FieldValidationResult.CreateError(LastNameProperty, "Необходима фамилия сотрудника"));
            }
        }

    }
}