using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class Group
    {
        [Key]
        public int GroupID { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}