using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class GroupMember
    {
        [Key, Column(Order = 0)]
        public string AppUser_ID { get; set; }
        [Key, Column(Order = 1)]
        public int Group_ID { get; set; }

        [InverseProperty("Groups")]
        [ForeignKey("AppUser_ID")]
        public virtual ApplicationUser User { get; set; }

        [InverseProperty("Users")]
        [ForeignKey("Group_ID")]
        public virtual Group Group { get; set; }

        public string NickName { get; set; }

    }
}