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
    public class ListsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Lists
        public ActionResult Index()
        {
            return View(GetMyLists());
        }

        /// <summary>
        /// returns lists related to the current user
        /// </summary>
        /// <returns></returns>
        private IEnumerable<List> GetMyLists()
        {
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            IEnumerable<List> myLists = db.Lists.ToList().Where(x => x.User == currentuser);
            return myLists;
        }

        // GET: Lists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.Lists.Include(x => x.ToDos).Where(x => x.ListID == id).FirstOrDefault();
            
            if (list == null)
            {
                return HttpNotFound();
            }

            // return bad request is someone else's list is tried to be accessed. 
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (list.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(list);
        }

        // GET: Lists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ListID,ListName")] List list)
        {
            if (ModelState.IsValid)
            {
                // associates the list with the user creating it
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                list.User = currentuser;

                if (Request.QueryString["FolderID"] != null)
                {
                    var folderID = Convert.ToInt32(Request.QueryString["FolderID"]);
                    var folder = db.Folders.Find(folderID);
                    if (folder != null)
                    {
                        list.Folder = folder;
                    }
                }

                db.Lists.Add(list);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(list);
        }

        // GET: Lists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            // return bad request is someone else's list is tried to be accessed. 
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (list.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(list);
        }

        // POST: Lists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListID,ListName")] List list)
        {
            if (ModelState.IsValid)
            {
                db.Entry(list).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(list);
        }

        // GET: Lists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List list = db.Lists.Find(id);
            if (list == null)
            {
                return HttpNotFound();
            }

            // return bad request is someone else's list is tried to be accessed
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (list.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(list);
        }

        // POST: Lists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            List list = db.Lists.Find(id);

            /*List temporarylist = new List
            {
                ListID = -1
            };*/

            // get the todos inside the list
            IEnumerable<ToDo> ListToDoes = db.ToDos.ToList().Where(x => x.List != null && x.List.ListID == list.ListID);

            // remove all the todos inside the list if there are any
            /*if (!ListToDoes.Any())
            {
                for (int i = 0; i < ListToDoes.Count(); i++)
                {
                
                    ListToDoes.ElementAt(i).List = temporarylist;
                }
            }
            foreach (ToDo todo in db.ToDos.ToList().Where(x => x.List != null &&  x.List.ListID == -1))
            {
                db.ToDos.Remove(todo);
            }*/


            db.Lists.Remove(list);
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
