

using System.ComponentModel.DataAnnotations;

namespace SyZero.Domain.Model
{
    public class Tool : EntityRoot
    {
       
        [MaxLength(200)]
        public string Name { get; set; }
        public int Ctid { get; set; }
        [MaxLength(1000)]
        public string Url { get; set; }
        [MaxLength(200)]
        public string Tqm { get; set; }
        public string L01 { get; set; }

        public string L02 { get; set; }

        public string L03 { get; set; }
    }
}
