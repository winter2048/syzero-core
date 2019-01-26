
using System.ComponentModel.DataAnnotations;

namespace SyZero.Domain.Model
{
    public class UserMo : MongoEntity
    {
        public UserMo (){
        }

    
        public string Name { get; set; }
      
        public string Paw { get; set; }
       
        public string Mail { get; set; }
       
        public string Phone { get; set; }
     
        public string Headimg { get; set; }
        public int Utype { get; set; }
     
        public string Sex { get; set; }
        public System.DateTime AddTime { get; set; }
        public System.DateTime LastTime { get; set; }
        public int State { get; set; }
    }
}
