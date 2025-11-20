using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class AdminEntity
    {
        public int? AdminID { get; set; } = null;
        [MaxLength(255)]
        public required string EmailAddress { get; set; }
        [MaxLength(255)]
        public required string Password { get; set; }
        [MaxLength(255)]
        public required string FirstName { get; set; }
        [MaxLength(255)]
        public required string LastName { get; set; }
   
    }
}
