using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


namespace u22560646_HW3.Models
{
    public class Stock
    {
        [Key, Column(Order = 0)]
        public int StoreId { get; set; }

        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}