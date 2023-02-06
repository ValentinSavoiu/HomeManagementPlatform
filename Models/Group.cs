using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class Group
    {
        [Key]
        public int GroupID { get; set; }

        public string Name { get; set; }
        // foreing key - gaseste pe net
        public string OwnerEmail { get; set; }
        // adaug si o lista de membrii
        //public List<Member> 
        public virtual ICollection<GroupMember> Users { get; set; }

		[InverseProperty("Group")]
		public virtual ICollection<Ticket> Tickets { get; set; }
	}
}
// caut userul dupa email
// ii iau id-ul
// creed un entry cu aplication user group