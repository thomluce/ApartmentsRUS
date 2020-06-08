using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ApartmentsRUS.DAL;
using ApartmentsRUS.Models;
using Microsoft.AspNet.Identity;

namespace ApartmentsRUS.Controllers
{
    [Authorize]
    public class RegisteredUsersController : Controller
    {
        private Context db = new Context();

        // GET: RegisteredUsers
        public ActionResult Index()
        {
            return View(db.registeredUsers.ToList());
        }

        // GET: RegisteredUsers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.registeredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            return View(registeredUser);
        }

        // GET: RegisteredUsers/Create
        public ActionResult Create()
        {
            // get the ID of the current user and save it in TempData
            //Guid memberId;
            //Guid.TryParse(User.Identity.GetUserId(), out memberId);
            //TempData["currentUser"] = memberId;

            return View();
        }
        // POST: RegisteredUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,lastName,firstName,email,registrationDate,role")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                Guid memberId;
                Guid.TryParse(User.Identity.GetUserId(), out memberId);
                registeredUser.Id = memberId;
                // retrieve the user ID from TempData, cast it back to a Guid and store in the model
                //registeredUser.Id = (Guid)TempData["currentUser"];
                registeredUser.email = User.Identity.Name;
                registeredUser.registrationDate = DateTime.Now;
                registeredUser.role = RegisteredUser.roles.visitor;
                db.registeredUsers.Add(registeredUser);
                try
                {
                    db.SaveChanges();
                }
                catch
                {
                    return View("DuplicateUser");
                }
                return RedirectToAction("Index");
            }

            return View(registeredUser);
        }

        // GET: RegisteredUsers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.registeredUsers.Find(id);

            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;

            if (memberId == id || isAdmin)
            {
                return View(registeredUser);
            }
            else
            {
                return View("notAuthorized");
            }
        }

        // POST: RegisteredUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,lastName,firstName,email,registrationDate,role")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registeredUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registeredUser);
        }

        // GET: RegisteredUsers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.registeredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
            if (isAdmin)
            {
                return View(registeredUser);
            }
            else
            {
                return View("AdminOnly");
            }
        }

        // POST: RegisteredUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RegisteredUser registeredUser = db.registeredUsers.Find(id);
            db.registeredUsers.Remove(registeredUser);
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
