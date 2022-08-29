using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    [Table("License")]
    public class License
    {
        
        
            [Key]
            public int Id { get; set; }
            public string UserEmail { get; set; }
            public string LicenseName { get; set; }
            public string Month { get; set; }
            public int Age { get; set; }
    }
    
}
