using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id, bool includeDirectReports = true)
        {
            //Get queryable employees, load direct reports
            var employees = _employeeContext.Employees.Include(e => e.DirectReports);
     
            return employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        /**
         * Gets the count of all reports under an employee
         * @param {employee} employee
         * @returns {int} the count of reports
         */
        public int GetCountAllReports(Employee employee)
        {
            var set = this.GetAllReportsSet(employee);

            return set.Count();
        }

        /**
         * Recursively maps and calculates the count of all reports under an employee
         * @param {employee} employee
         * @param {HashSet<string>} reports
         * @returns {HashSet<string>} A hashset of all the unique EmployeeIds reporting up to the given employee
         */
        private HashSet<string> GetAllReportsSet(Employee employee, HashSet<string> reports = null)
        {
            if(reports == null)
            {
                reports = new HashSet<string>();
            }

            if(employee.DirectReports != null)
            {
                //Run through every report for the employee, adding their ID to the hashset (enforces uniqueness)
                foreach(var report in employee.DirectReports)
                {
                    //Specifically load values for direct reports, which are lazy loaded
                    _employeeContext.Entry(report).Navigation("DirectReports").Load();
                    reports.Add(report.EmployeeId);

                    //Process all reports for this employee
                    GetAllReportsSet(report, reports);
                }
            }

            return reports;
        }
    }
}
