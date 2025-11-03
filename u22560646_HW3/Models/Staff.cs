using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace u22560646_HW3.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        public byte Active { get; set; }

        public int StoreId { get; set; }

        public int? ManagerId { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }
    }
}