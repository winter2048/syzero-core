using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SyZero.Domain.Model
{
    public class FriendLink : EntityRoot
    {
       
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(1000)]
        public string Href { get; set; }
        [MaxLength(200)]
        public string Category { get; set; }

        public string L01 { get; set; }

        public string L02 { get; set; }

        public string L03 { get; set; }
    }
}
