using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    [Table("E1tables")]
    public class E1table
    {
        public DateTime DateOfApplication { get; set; } //День
        public int Id { get; set; }
       
        public string t1{ get; set; } // Код направления
        public int d1 { get; set; }
        public string t2 { get; set; } // Код направления
        public int d2 { get; set; }
        public string t3 { get; set; } // Код направления
        public int d3 { get; set; }
        public string t4 { get; set; } // Код направления
        public int d4 { get; set; }
        public string t5 { get; set; } // Код направления
        public int d5 { get; set; }
        public string t6 { get; set; } // Код направления
        public int d6 { get; set; }
        public string t7 { get; set; } // Код направления
        public int d7 { get; set; }
        public string t8 { get; set; } // Код направления
        public int d8 { get; set; }
        public string t9 { get; set; } // Код направления
        public int d9 { get; set; }
        public string t10 { get; set; } // Код направления
        public int d10 { get; set; }

    }
}
