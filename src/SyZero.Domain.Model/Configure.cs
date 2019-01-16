using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SyZero.Domain.Model
{
    public class Configure : EntityRoot
    {
       
        [MaxLength(200)]
        public string Category { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        [MaxLength(200)]
        public string Other { get; set; }
        public string L01 { get; set; }

        public string L02 { get; set; }

        public string L03 { get; set; }
    }
}
