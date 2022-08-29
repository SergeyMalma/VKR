using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Models
{
    public class UserFile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FileName { get; set; }
        public byte[] FileObject { get; set; }
    }
}
