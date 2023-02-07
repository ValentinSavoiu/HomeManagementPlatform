using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mss_project.Models
{
    public class Comment
	{
        [Key]
        public int CommentID { get; set; }

		public int? PreviousCommentID { get; set; }
		[ForeignKey("PreviousCommentID")]
		public Comment PreviousComment { get; set; }

		public int TicketID{ get; set; }

		[InverseProperty("Comments")]
		[ForeignKey("TicketID")]
		public virtual Ticket Ticket { get; set; }

		public string CreatorID { get; set; }
        
		[InverseProperty("CreatedComments")]
        [ForeignKey("CreatorID")]
		public virtual ApplicationUser CommentCreator { get; set; }

		[Required(ErrorMessage = "Text is required"), StringLength(500, ErrorMessage = "The comment cannot have more than 500 characters")]
		public string Text { get; set; }

		public DateTime DateCreated { get; set; }
	}
}