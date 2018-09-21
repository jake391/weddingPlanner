using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using weddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace weddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly Context _context;
 
        public HomeController(Context context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View(); 
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid){

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.password = Hasher.HashPassword(user, user.password);

                _context.Add(user);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("Id", (int)user.UserId);
                return RedirectToAction("Dashboard");
            }
            else {
                return View("Index");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            var user = _context.users.SingleOrDefault(users => users.email == email);
            if(user != null && password != null)
            {
                var Hasher = new PasswordHasher<User>();
                var result = Hasher.VerifyHashedPassword(user, user.password, password);
                if(result != 0)
                {
                    HttpContext.Session.SetInt32("Id", (int)user.UserId);

                    return RedirectToAction("Dashboard");
                }
                else{
                    
                    return View("Index");
                }
            }
            else{
                return View("Index");
            }    
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            int? idd = HttpContext.Session.GetInt32("Id");
            var user = _context.users.SingleOrDefault(u => u.UserId == idd);
            ViewBag.idd = idd;

            ViewModel viewModel = new ViewModel();

            List<Wedding> AllWeddings = new List<Wedding>();
            // List<User> guestsGoing = new List<User>();
            // List<User> guestsNotGoing = new List<User>();
            List<Rsvp> Going = new List<Rsvp>();
            // List<Rsvp> Going = _context.rsvp.Where(x=>x.UserId==idd).ToList();
            

            // List<User> Guest = _context.users
            //         .Include(u => u.Weddings)
            //             .ThenInclude(r => r.Wedding)
            //         .ToList();

            // foreach(var x in Guest){
            //     if(x.Weddings.SingleOrDefault(g => g.UserId == idd) != null){
            //         guestsGoing.Add(x);
            //     }
            //     else {
            //         guestsNotGoing.Add(x);
            //     }
            // }

            viewModel.AllWeddings = _context.weddings.Include(w => w.Users).ToList();
        
            // viewModel.guestsNotGoing = guestsNotGoing;
            // viewModel.guestsGoing = guestsGoing;
            viewModel.Going = Going;
            // viewModel.AllGuests = AllGuests;
            

            return View(viewModel);
        }

        [HttpGet("newWedding")]
        public IActionResult newWedding()
        {

            int? idd = HttpContext.Session.GetInt32("Id");
            var user = _context.users.SingleOrDefault(u => u.UserId == idd);
            return View("NewWedding");
        }

        [HttpPost("createWedding")]
        public IActionResult createWedding(Wedding wedding)
        {
            ViewModel ViewToIndex = new ViewModel();
            ViewToIndex.AllUsers = GetUsers();

            if(ModelState.IsValid){
                
                int? idd = HttpContext.Session.GetInt32("Id");
                var user = _context.users.SingleOrDefault(u => u.UserId == idd);

                // wedding.User = _context.users.Where(u => u.UserId == wedding.UserId).FirstOrDefault();
                // Wedding weddings = _context.weddings.SingleOrDefault(w => w.UserId == wedding.UserId);
                // User user = _context.users.SingleOrDefault(u => u.UserId == wedding.UserId);
                
                wedding.User = user;
                

                _context.weddings.Add(wedding);
                _context.SaveChanges();

                Console.WriteLine(wedding.UserId + "===================");

            return RedirectToAction("Wedding", new{wId = wedding.WeddingId});
            }
            else {
                return View("newWedding");
            }
        }

        [HttpGet("Wedding/{wId}")]
        public IActionResult Wedding(int wId)
        {
            Wedding wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == wId);

            ViewModel viewModel = new ViewModel();

            viewModel.guestsGoing = Guests();

            List<User> guestsGoing = new List<User>();

            List<Rsvp> Going = _context.rsvp.Where(x=>x.WeddingId==wId).ToList();

            // List<User> Guest = _context.users
            //         .Include(u => u.Weddings)
            //             .ThenInclude(r => r.Wedding)
            //         .ToList();

            // foreach(var x in Guest){
            //     if(x.Weddings.SingleOrDefault(g => g.UserId == wId) != null){
            //         guestsGoing.Add(x);
            //     }
            // }

            viewModel.guestsGoing = guestsGoing;
            viewModel.Going = Going;

            int? idd = HttpContext.Session.GetInt32("Id");
            var user = _context.users.SingleOrDefault(u => u.UserId == idd);
            var wed = wedding.WeddingId;

            ViewBag.wedderOne = wedding.wedderOne;
            ViewBag.wedderTwo = wedding.wedderTwo + "'s";
            ViewBag.first_name = user.first_name;
            ViewBag.last_name = user.last_name;
            ViewBag.date = wedding.date;

            return View(viewModel);
        }

        [HttpGet("Delete/{wId}")]
        public IActionResult Delete(int wId)
        {
            Wedding wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == wId);
            _context.weddings.Remove(wedding);
            _context.SaveChanges();

            return RedirectToAction("Dashboard");
        }

        
        public IActionResult logOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("Rsvp/{wId}")]
        public IActionResult Rsvp(int wId)
        {
            int? idd = HttpContext.Session.GetInt32("Id");
            var user = _context.users.SingleOrDefault(u => u.UserId == idd);
            Rsvp newRsvp = new Rsvp();
            newRsvp.User = user;

            Wedding wedding = _context.weddings.SingleOrDefault(w => w.WeddingId == wId);
            newRsvp.Wedding = wedding;

            _context.rsvp.Add(newRsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("UnRsvp/{wId}")]
        public IActionResult UnRsvp(int wId)
        {
            int? idd = HttpContext.Session.GetInt32("Id");

            Rsvp rsvp = _context.rsvp.SingleOrDefault(w => w.WeddingId == wId && w.UserId == idd);
            

            _context.rsvp.Remove(rsvp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        private List<User> GetUsers()
        {
            return _context.users.ToList();
        }

        private List<User> Guests()
        {
            return _context.users.ToList();
        }

        private List<Wedding> Weddings()
        {
            return _context.weddings.ToList();
        }

    }
}
