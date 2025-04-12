using System.Collections.Generic;
using System.Threading.Tasks;
using ERMS.Models;

namespace ERMS.Repositories
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        bool EmployeeExists(int id);
    }
}
