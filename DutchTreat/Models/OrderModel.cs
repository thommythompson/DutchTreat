using DutchTreat.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }   
        public DateTime OrderDate { get; set; }
        [Required]
        [MinLength(4)]
        public string OrderNumber { get; set; }
        public ICollection<OrderItemModel> Items { get; set; }
    }
}
