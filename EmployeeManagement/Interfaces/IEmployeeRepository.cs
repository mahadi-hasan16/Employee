using EmployeeManagement.Models.Entities;

namespace EmployeeManagement.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmploiesAsync();
        Task AddEmployeeAsync(Employee newEmployee);
        IQueryable<Employee> EmployeeQuery();
        Task<Employee> FindEmployeeAsync(int id);
        void UpdateEmployeeInfo(Employee employee);
        void RemoveEmployee(int id);
    }
}
