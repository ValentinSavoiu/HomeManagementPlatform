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

        public int CreatorID { get; set; }
        public virtual Member Creator { get; set; }
        
        [Required(ErrorMessage = "Description is required"), StringLength(500, ErrorMessage = "The description cannot have more than 500 characters")]
        public string Description { get; set; }

        public int AssigneeID { get; set; }
        public virtual Member Assignee { get; set; }

       

    }
}