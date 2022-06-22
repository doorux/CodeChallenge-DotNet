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
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRespository(ILogger<ICompensationRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        /**
         * Creates a new compensation for an employee
         * @param {Compensation}
         * @returns {Compensation}
         */
        public Compensation Add(Compensation compensation)
        {
            var c = _employeeContext.Compensation.Add(compensation).Entity;
            return c;
        }

        /**
         * Gets the compensation for an employee
         * @param {string} employeeId
         * @returns {Compensation}
         */
        public Compensation GetByEmployeeId(string id)
        {
            var compensations = _employeeContext.Compensation.Include(c => c.Employee).AsQueryable();

            var compensation = compensations.SingleOrDefault(c => c.EmployeeID == id);

            return compensation;
        }

        /**
         * Asynchronously saves the changes to the context
         */
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
