using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingPlanner.Models
{
    public class Rsvp
    {
        [Key]
        public int RsvpId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}