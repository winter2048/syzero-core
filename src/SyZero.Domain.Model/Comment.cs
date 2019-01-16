

using System.ComponentModel.DataAnnotations;

namespace SyZero.Domain.Model
{
    public class Comment : EntityRoot
    {
      
        public int Userid { get; set; }
        public int Artid { get; set; }
        [MaxLength(1000)]
        public string Content { get; set; }
        public System.DateTime AddTime { get; set; }
        public int Pabulous { get; set; }
        public int Parent { get; set; }
        public string L01 { get; set; }

        public string L02 { get; set; }

        public string L03 { get; set; }
    }
}
