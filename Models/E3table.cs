using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    [Table("E3tables")]
    public class E3table
    {
        public int id { get; set; }
        public int abitur_id { get; set; }
        public int scores_1 { get; set; }
        public int scores_2 { get; set; }
        public int scores_3 { get; set; }
        public int total_scores { get; set; }
        public string ex_type { get; set; }
        public string SN { get; set; }



    }
}
