using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class Ticket
    {
        [Key]
        public int TicketID { get; set; }

        [Required(ErrorMessage = "Title is required"), StringLength(100, ErrorMessage = "The title cannot have more than 100 characters")]
        public string Title { get; set; }

		public int GroupID{ get; set; }

		[InverseProperty("Tickets")]
		[ForeignKey("GroupID")]
		public virtual Group Group { get; set; }

		public string CreatorID { get; set; }
        
		[InverseProperty("CreatedTickets")]
        [ForeignKey("CreatorID")]
		public virtual ApplicationUser Creator { get; set; }
        
        [Required(ErrorMessage = "Description is required"), StringLength(500, ErrorMessage = "The description cannot have more than 500 characters")]
        public string Description { get; set; }

		public TicketStatus Status { get; set; }

        [InverseProperty("AssignedTickets")]
		public virtual ICollection<ApplicationUser> Assignees { get; set; }

		[InverseProperty("Ticket")]
		public virtual ICollection<Comment> Comments { get; set; }
	}

    public enum TicketStatus
    {
        [Display(Name = "Not Started")]
        NotStarted,
		[Display(Name = "In Progress")]
		InProgress,
		[Display(Name = "Completed")]
		Completed
    }
}