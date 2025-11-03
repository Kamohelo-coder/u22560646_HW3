using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required, StringLength(100)]
        public string BrandName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}