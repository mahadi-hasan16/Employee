namespace EmployeeManagement.Interfaces
{
    public interface IUnitOfWork
    {
        IEmployeeRepository EmployeeRepository { get; }
        Task SaveChangesAsync();
    }
}
