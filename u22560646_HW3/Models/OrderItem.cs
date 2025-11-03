using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class OrderItem
    {
        [Key, Column(Order = 0)]
        public int OrderId { get; set; }

        [Key, Column(Order = 1)]
        public int ItemId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal ListPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Discount { get; set; }
        // Navigation Properties
        public virtual Order Order { get; set; }
        

        // THIS IS THE MISSING LINE
        public virtual Product Product { get; set; }
    }
}