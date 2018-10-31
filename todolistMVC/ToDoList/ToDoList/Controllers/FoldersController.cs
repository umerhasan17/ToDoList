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
    public class FoldersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Folders
        public ActionResult Index()
        {

            return View(GetMyFolders());
        }

        /// <summary>
        /// Gets the folders related only to the current user. The index methods calls this function. 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Folder> GetMyFolders()
        {
            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            IEnumerable<Folder> myFolders = db.Folders.ToList().Where(x => x.User == currentuser);
            return myFolders;
        }

        // GET: Folders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (folder.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(folder);
        }

        // GET: Folders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Folders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FolderID,FolderName")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                // associates the folder with the user creating it
                string currentUserID = User.Identity.GetUserId();
                ApplicationUser currentuser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
                folder.User = currentuser;

                db.Folders.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(folder);
        }

        // GET: Folders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (folder.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(folder);
        }

        // POST: Folders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FolderID,FolderName")] Folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(folder);
        }

        // GET: Folders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Folder folder = db.Folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }

            string currentUserID = User.Identity.GetUserId();
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserID);
            if (folder.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(folder);
        }

        // POST: Folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Folder folder = db.Folders.Find(id);
            // get the todos inside the list
            IEnumerable<List> FolderLists = db.Lists.ToList().Where(x => x.Folder != null && x.Folder.FolderID == folder.FolderID);
            // remove all the todos inside the list if there are any
            if (!FolderLists.Any())
            {
                for (int i = 0; i < FolderLists.Count(); i++)
                {
                    db.Lists.Remove(FolderLists.ElementAt(i));
                }
            }
            db.Folders.Remove(folder);
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
