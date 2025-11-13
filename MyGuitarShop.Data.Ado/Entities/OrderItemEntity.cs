using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Data.Ado.Entities
{
    public class OrderItemEntity
    {
        public int? ItemID { get; set; } = null;
        public int? OrderID { get; set; } = null;
        public int? ProductID { get; set; } = null;
        public required decimal ItemPrice { get; set; }
        public required decimal DiscountAmount { get; set; } = 0.0m;
        public required int Quantity { get; set; }
    }
}
