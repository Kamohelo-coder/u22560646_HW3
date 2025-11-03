using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int? CustomerId { get; set; }

        public byte OrderStatus { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public DateTime? ShippedDate { get; set; }

        public int StoreId { get; set; }

        public int? StaffId { get; set; }

        
        public virtual Customer Customer { get; set; }
        public virtual Staff Staff { get; set; }
        public virtual Store Store { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}