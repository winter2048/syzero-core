using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SyZero.Domain.Model
{
    public class TimeAxis : EntityRoot
    {
    
        public System.DateTime Releasetime { get; set; }
        [MaxLength(200)]
        public string Releasetitle { get; set; }
        [MaxLength(200)]
        public string Brief { get; set; }
    }
}
