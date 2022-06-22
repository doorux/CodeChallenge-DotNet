using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee        GetById(String id, bool includeDirectReports = true);
        Employee        Add(Employee employee);
        Employee        Remove(Employee employee);
        Task            SaveAsync();

        int             GetCountAllReports(Employee employee);
    }
}