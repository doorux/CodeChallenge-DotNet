using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        [ForeignKey("Employee")]
        public string EmployeeID { get; set; }
        public decimal Salary { get; set; }
        public DateTimeOffset EffectiveDate { get; set; }
        
        public virtual Employee Employee { get; set; }
    }
}
