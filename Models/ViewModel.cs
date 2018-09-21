using System.Collections.Generic;

namespace weddingPlanner.Models
{
    public class ViewModel
    {
        public User User { get; set; }
        public Wedding Wedding { get; set; }
        public Rsvp Rsvp { get; set; }

        public List<User> AllUsers = new List<User>();

        public List<Wedding> AllWeddings = new List<Wedding>();

        public List<User> guestsGoing = new List<User>();

        public List<User> guestsNotGoing = new List<User>();

        public List<Rsvp> Going = new List<Rsvp>();

        public List<Rsvp> AllGuests = new List<Rsvp>();
    }
}