using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace u22560646_HW3.Models
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        [StringLength(150)]
        public string StoreName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(100)]
        public string State { get; set; }

        [StringLength(20)]
        [Column("zip_code")]
        public string ZipCode { get; set; }

    }
}