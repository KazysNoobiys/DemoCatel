using Demo.Models.DAL;

namespace Demo.ViewModels
{
    public class IncludeEmployee
    {
        public Employee Employee { get; set; }
        public bool IsInclude { get; set; }

        public IncludeEmployee(Employee employee, bool b)
        {
            Employee = employee;
            IsInclude = b;
        }
    }
}