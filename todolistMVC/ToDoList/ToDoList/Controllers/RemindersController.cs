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
    public class RemindersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reminders
        public ActionResult Index()
        {
            return View(GetMyReminders());
        }

        private IEnumerable<ToDo> GetMyReminders()
        {
            // get the current user ID
            string currentUserID = User.Identity.GetUserId();
            // find the user in the user table
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            // return the uncompleted ToDos associated with the specific user ID which have the reminder date for today.
            IEnumerable<ToDo> myToDoes = db.ToDos.ToList().Where(x => x.User == currentUser && x.IsDone == false && x.ReminderDate == DateTime.Today);
            // order by due date
            myToDoes = myToDoes.OrderBy(x => x.DueDate);

            return myToDoes;
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
