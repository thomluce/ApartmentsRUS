using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApartmentsRUS.DAL;
using ApartmentsRUS.Models;
using Microsoft.AspNet.Identity;

namespace ApartmentsRUS.Controllers
{

    [Authorize]
    public class BuildingsController : Controller
    {
        private Context db = new Context();

        // GET: Buildings
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.building.ToList());
        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.building.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        public ActionResult Create()
        {
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            if (loggedInUser != null) // need to check if we found a user
            {
                bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
                bool isOwner = loggedInUser.role == RegisteredUser.roles.owner;
                if (isAdmin || isOwner)
                {
                    return View();
                }
                else
                {
                    return View("~/Views/Owners/RestrictedOperation.cshtml");
                }
            }
            else
            {
                return View("~/Views/Owners/MissingRegistration.cshtml");
            }
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "buildingID,street,city,state,zip,inspectionDate,inspectionResults,appraisedValue,propertyTaxRate")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.building.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.building.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            if (loggedInUser != null) // need to check if we found a user
            {
                bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
                bool isOwner = loggedInUser.role == RegisteredUser.roles.owner;
                if (isAdmin || isOwner)
                {
                    return View(building);
                }
                else
                {
                    return View("~/Views/Owners/RestrictedOperation.cshtml");
                }
            }
            else
            {
                return View("~/Views/Owners/MissingRegistration.cshtml");
            }
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "buildingID,street,city,state,zip,inspectionDate,inspectionResults,appraisedValue,propertyTaxRate")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(building);
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.building.Find(id);
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.building.Find(id);
            db.building.Remove(building);
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
