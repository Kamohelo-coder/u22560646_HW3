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
        public int ProductId { get; set; }         

        [Required, StringLength(255)]
        public string ProductName { get; set; }   

        public int BrandId { get; set; }          
        public int CategoryId { get; set; }       
        public short ModelYear { get; set; }      

        [Column(TypeName = "decimal")]
        public decimal ListPrice { get; set; }    

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}