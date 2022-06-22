using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class CompensationRequest
    {
        public string EmployeeID { get; set; }
        public decimal Salary { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }

    }
}
