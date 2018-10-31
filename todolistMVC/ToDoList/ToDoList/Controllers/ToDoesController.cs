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
    public class ToDoesController : Controller
    {
        // This is the Data Access Layer, used for adding, editing, deleting from and to the database, based on user input. 
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ToDoes
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// returns todos related to the current user
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ToDo> GetMyToDoes()
        {
            
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);

            IEnumerable<ToDo> myToDoes = db.ToDos.ToList().Where(x => x.User == currentuser);

            int completecount = 0;

            foreach (ToDo toDo in myToDoes)
            {
                if (toDo.IsDone)
                {
                    completecount++;
                }
            }

            // View bag for the progress bar which is sent to the view so we can use it. The Viewbag in this case is a single number i.e. the percentage
            ViewBag.Percent = Math.Round(100f * ((float)completecount / (float)myToDoes.Count()));

            return db.ToDos.ToList().Where(x => x.User == currentuser);
        }

        // This controller builds the ToDoTable which is then added to the Index, the table is not directly inputted into the Index. The Index is rather a collection of several partial views. 
        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", GetMyToDoes());
        }

        // GET: ToDoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDo);
        }

        // GET: ToDoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ToDoID,Description,DueDate,ReminderDate,PriorityLevel,Lat,Lon,Duration")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // when a new ToDo is created - there needs to be a UserID assigned to it.
                string currentUserID = User.Identity.GetUserId();
                // get the UserID from the database.
                ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                // associate the retrieved UserID with the ToDo object being created. 
                toDo.User = currentuser;
                // we just created the ToDo hence we know it is not done. 
                toDo.IsDone = false;
                if(Request.QueryString["ListId"] != null)
                {
                    var listId = Convert.ToInt32(Request.QueryString["ListId"]);
                    var list = db.Lists.Find(listId);
                    if(list != null)
                    {
                        toDo.List = list;
                    }
                }
                
                db.ToDos.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        /// <summary>
        /// This extra create function enables us to add quick ToDos using AJAX. ToDo will then be added without the need for page reloads. 
        /// </summary>
        /// <param name="toDo"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "ToDoID,Description,DueDate")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                // when a new ToDo is created - there needs to be a UserID assigned to it.
                string currentUserID = User.Identity.GetUserId();
                // get the UserID from the database.
                ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                // associate the retrieved UserID with the ToDo object being created. 
                toDo.User = currentuser;
                // we just created the ToDo hence we know it is not done. 
                toDo.IsDone = false;
                toDo.PriorityLevel = 3;
                db.ToDos.Add(toDo);
                db.SaveChanges();

            }

            return PartialView("_ToDoTable", GetMyToDoes());
        }

        // GET: ToDoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);

            if (toDo == null)
            {
                return HttpNotFound();
            }

            // This section of code means that that another user can't just copy and paste the url in. If the UserIDs do not match, the user will be given a bad request. 
            // We know this can't happen while logged off since before navigating to any page, the user is required to login. s
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDo);
        }

        // POST: ToDoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ToDoID,Description,IsDone,DueDate,ReminderDate,PriorityLevel,Lat,Lon,Duration")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        /// <summary>
        /// This AJAX Edit method will enable the user to tick off To-Dos in the table view and have it saved to the database. Editing the rest of the parameters will be completed in a seperate pane. 
        /// </summary>
        /// <param name="toDo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            else
            {
                toDo.IsDone = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();

            }

            return PartialView("_ToDoTable", GetMyToDoes());
        }

        public ActionResult GetCoordinates(ToDo myToDo)
        {
            return PartialView("_Coodinates", myToDo);
        }



        // GET: ToDoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDos.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDo);
        }

        // POST: ToDoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDo toDo = db.ToDos.Find(id);
            db.ToDos.Remove(toDo);
            db.SaveChanges();
            return RedirectToAction("Index");
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
