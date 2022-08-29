using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    [Table("E2tables")]
    public class E2table
    {
        public int id { get; set; }
        public int d1 { get; set; }

        public int d2 { get; set; }

        public int d3 { get; set; }

        public int d4 { get; set; }

        public int d5 { get; set; }

        public int d6 { get; set; }

        public int d7 { get; set; }

        public int d8 { get; set; }

        public int d9 { get; set; }

        public int d10 { get; set; }
        public DateTime DateOfApplication { get; set; } //День
        public int s1 { get; set; }
        public int s2 { get; set; }
        public int s3 { get; set; }
        public int s4 { get; set; }
        public int s5 { get; set; }
        public int s6 { get; set; }
        public int s7 { get; set; }
        public int s8 { get; set; }
        public int s9 { get; set; }
        public int s10 { get; set; }

    }
}
