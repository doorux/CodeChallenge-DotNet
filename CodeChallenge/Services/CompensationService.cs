using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository        _compensationRepository;
        private readonly IEmployeeRepository            _employeeRepository;
        private readonly ILogger<CompensationService>   _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeRepository employeeRepository)
        {
            _compensationRepository = compensationRepository;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Compensation Create(CompensationRequest compensationReq)
        {
            Compensation compensationResponse = new Compensation();

            Employee employee;
            if(compensationReq != null)
            {
                //Fetch employee for attaching to compensation
                employee = _employeeRepository.GetById(compensationReq.EmployeeID);

                //Verify Employee Exists
                if (employee == null)
                {
                    throw new Exception("Invalid EmployeeID or EmployeeID not found");
                }

                //Construct new Compensation model
                var compensation = new Compensation()
                {
                    Employee = null,
                    Salary = compensationReq.Salary,
                    EffectiveDate = compensationReq.EffectiveDate,
                    EmployeeID = compensationReq.EmployeeID
                };

                //Add to DB and save
                compensationResponse = _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }
            return compensationResponse;
        }

        public Compensation GetByEmployeeId(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _compensationRepository.GetByEmployeeId(id);
            }

            return null;
        }
    }
}
