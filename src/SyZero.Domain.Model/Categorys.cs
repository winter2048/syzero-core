
using System.ComponentModel.DataAnnotations;

namespace SyZero.Domain.Model
{
    public class Categorys: EntityRoot
    {
        [MaxLength(200), Required]
        public string Category { get; set; }
        [MaxLength(200), Required]
        public string Name { get; set; }

        public int Parentid { get; set; }
    }
}
