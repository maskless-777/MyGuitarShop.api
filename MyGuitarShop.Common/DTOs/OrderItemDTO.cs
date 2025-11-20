using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MyGuitarShop.Common.DTOs
{
    public class OrderItemDTO
    {
        public int? ItemID { get; set; } = null;
        public int? OrderID {  get; set; } = null;
        public int? ProductID {  get; set; } = null;
        public required decimal ItemPrice { get; set; }
        public required decimal DiscountAmount { get; set; } = 0.0m;
        public required int Quantity { get; set; }
    }
}
