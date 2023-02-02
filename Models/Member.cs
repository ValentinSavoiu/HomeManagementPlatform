using mss_project.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }


        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [Required]
        [Name(ErrorMessage = "Names should only contain letters, spaces or hyphens. Sorry Elon")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Name(ErrorMessage = "Names should only contain letters, spaces or hyphens. Sorry Elon")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

		[InverseProperty("Creator")]
		public virtual ICollection<Ticket> CreatedTickets { get; set; }

		[InverseProperty("Assignees")]
        public virtual ICollection<Ticket> AssignedTickets { get; set; }
    }
}