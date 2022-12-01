using System;
using System.Collections.Generic;
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

        [Required(ErrorMessage = "Title is required"), StringLength(100, ErrorMessage = "I don't need the entire ticket")]
        public string Title { get; set; }

        public int CreatorID { get; set; }
        public virtual Member Creator { get; set; }

        public int AssigneeID { get; set; }
        public virtual Member Assignee { get; set; }

       

    }
}