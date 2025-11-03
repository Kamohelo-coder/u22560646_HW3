using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }         // product_id

        [Required, StringLength(255)]
        public string ProductName { get; set; }   // product_name

        public int BrandId { get; set; }          // brand_id
        public int CategoryId { get; set; }       // category_id
        public short ModelYear { get; set; }      // model_year

        [Column(TypeName = "decimal")]
        public decimal ListPrice { get; set; }    // list_price

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}