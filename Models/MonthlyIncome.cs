using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
   
        [Table("MonthlyIncome")]
        public class MonthlyIncome
        {
            [Key]
            public int Id { get; set; }
            public string Month { get; set; }
            public int Earnings { get; set; }
            public int SubscribedUsers { get; set; }
            public int UnsubscribedUsers { get; set; }
            public int SoldLicenses { get; set; }
    }
    
}
