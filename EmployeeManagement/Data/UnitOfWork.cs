using EmployeeManagement.Interfaces;
using EmployeeManagement.Repositories;

namespace EmployeeManagement.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEmployeeRepository EmployeeRepository => new EmployeeRepository(_dbContext);

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
