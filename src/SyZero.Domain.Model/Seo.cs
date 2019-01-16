using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SyZero.Domain.Model
{
    public class Seo : EntityRoot
    {
       
        public int Artid { get; set; }
        [MaxLength(200)]
        public string Keyword { get; set; }
        [MaxLength(200)]
        public string Describe { get; set; }
        public string L01 { get; set; }

        public string L02 { get; set; }

        public string L03 { get; set; }
    }
}
