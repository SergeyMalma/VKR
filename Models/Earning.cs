using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    [Table("Earnings")]
    public class Earning
    {
        [Key]
        public int OrderId { get; set; }
        public string UserName { get; set; }
        public int Payment { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
