using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingPlanner.Models
{
    public class User
    {
        [Key]
        public int? UserId { get; set; }
        [Required]
        [MinLength(2)]
        public string first_name { get; set; }
        [Required]
        [MinLength(2)]
        public string last_name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }
        [Compare("password")]
        [MaxLength(30)]
        [NotMapped]
        public string password_confirm { get; set; }

        // public DateTime created_at { get; set; }
        // public DateTime updated_at { get; set; }

        public List<Rsvp> Weddings { get; set; }

        public User()
        {
            Weddings = new List<Rsvp>();
            // created_at = DateTime.Now;
            // updated_at = DateTime.Now;
        }
    }
}