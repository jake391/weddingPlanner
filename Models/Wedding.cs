using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int? WeddingId { get; set; }

        [Required]
        public string wedderOne { get; set; }

        [Required]
        public string wedderTwo { get; set; }

        [Required]
        public DateTime date { get; set; }

        [Required]
        public string address { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public List<Rsvp> Users { get; set; }

        public Wedding()
        {
            Users = new List<Rsvp>();
            created_at = DateTime.Now;
            updated_at = DateTime.Now;
        }

    }

}