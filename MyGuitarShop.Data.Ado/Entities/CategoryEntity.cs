using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class CategoryEntity
    {
        public int? CategoryID { get; set; } = null;
        [MaxLength(255)]
        public required string CategoryName { get; set; }
    }
}
