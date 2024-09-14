using EmployeeManagement.Data;
using EmployeeManagement.Interfaces;
using EmployeeManagement.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public EmployeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddEmployeeAsync(Employee newEmployee)
        {
            await _dbContext.EmployeeInfo.AddAsync(newEmployee);
        }

        public async Task<Employee> FindEmployeeAsync(int id)
        {
            var employee = await _dbContext.EmployeeInfo.FindAsync(id);
            return employee;
        }

        public async Task<IEnumerable<Employee>> GetAllEmploiesAsync()
        {
            var employees = await _dbContext.EmployeeInfo.ToListAsync();
            return employees;
        }

        public async void RemoveEmployee(int id)
        {
            var employee = await _dbContext.EmployeeInfo.FindAsync(id);

            _dbContext.EmployeeInfo.Remove(employee);
        }

        public void UpdateEmployeeInfo(Employee employee)
        {
            _dbContext.EmployeeInfo.Update(employee);
        }

        public IQueryable<Employee> EmployeeQuery()
        {
            var employees = _dbContext.EmployeeInfo.AsQueryable();
            return employees;
        }
    }
}
