

using System.ComponentModel.DataAnnotations;

namespace SyZero.Domain.Model
{
    public class UserType : EntityRoot
    {
        [MaxLength(200), Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Authority { get; set; }
    }
}
