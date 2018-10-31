using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ToDoList.Models;
using Microsoft.AspNet.Identity;

namespace ToDoList.Controllers
{
    public class SuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suggestions
        [HttpGet]
        public ActionResult Index()
        {
            return View(db.ToDos.ToList());
        }

        private IEnumerable<ToDo> BuildToDoList()
        {
            // get the current user ID
            string currentUserID = User.Identity.GetUserId();
            // find the user in the user table
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            // return the uncompleted ToDos associated with the specific user ID
            IEnumerable<ToDo> myToDoes = db.ToDos.ToList().Where(x => x.User == currentUser && x.IsDone == false);

            return myToDoes;

        }
        
        [HttpPost, RequireHttps, ValidateAntiForgeryToken]
        public ActionResult GetMyGeneralSuggestions()
        {

            IEnumerable<ToDo> myToDoes = BuildToDoList();

            return View("GeneralSuggestions", myToDoes);

        }

        [HttpPost , RequireHttps , ValidateAntiForgeryToken]
        public ActionResult GetMyDurationSuggestions(TimeSpan durationInput)
        {
            // exception handling
            if (durationInput == null)
            {
                return Content("Please enter a value for the time you have.");
            }


            IEnumerable<ToDo> myToDoes = BuildToDoList();

            // gets the User's ToDos which have a value for the duration
            IEnumerable<ToDo> durationToDo = myToDoes.Where(x => x.Duration != null);

            // gets the User's ToDos whose durations are less than the time inputted by the user
            IEnumerable<ToDo> durationToDo2 = durationToDo.Where(x => x.Duration <= durationInput);

            // order ToDos by duration
            durationToDo2 = durationToDo2.OrderBy(x => x.Duration);

            if (!durationToDo2.Any())
            {
                return Content("Unfortunately, you are not able to do anything in the time you have. How about adding some extra information for the ToDos you have in there already?");
            }

            return View("DurationSuggestions", durationToDo2);
            
        }

        [HttpPost, RequireHttps, ValidateAntiForgeryToken]
        public ActionResult GetMyLocationSuggestions(decimal latitude, decimal longitude)
        {
            // in case the user does not enter location values
            if (latitude.ToString() == null || longitude.ToString() == null || latitude.ToString() == "" || longitude.ToString() == "")
            {
                return Content("Full location was not given");
            }


            IEnumerable<ToDo> myToDoes = BuildToDoList();

            // only get the ToDos within 1 decimal place of the current location which is passed into the controller from the view.

            // this line checks latitude
            IEnumerable<ToDo> locationToDo = myToDoes.Where(x => Math.Round(Convert.ToDecimal(x.Lat), 1) == Math.Round(latitude, 1));
            // this line checks longitude
            IEnumerable<ToDo> locationToDo2 = locationToDo.Where(x => Math.Round(Convert.ToDecimal(x.Lon), 1) == Math.Round(longitude, 1));

            if (!locationToDo2.Any())
            {
                return Content("Unfortunately, you do not have anything to do that is close by. How about adding some extra information for the ToDos you have in there already?"); 
            }

            return View("LocationSuggestions", locationToDo2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
