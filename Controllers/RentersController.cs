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
using Microsoft.Ajax.Utilities;

using PagedList;


namespace ApartmentsRUS.Controllers
{
    [Authorize]
    public class RentersController : Controller
    {
        private ApartmentsRUS.DAL.Context db = new ApartmentsRUS.DAL.Context();

        // GET: Renters
        public ActionResult Index(int? page, string searchString)
        {
            int pgSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.search = String.IsNullOrEmpty(searchString) ? "" : searchString;

            var renter = from r in db.renter select r;
            // sort the records
            renter = db.renter.OrderBy(r => r.lastName).ThenBy(r => r.firstName); 

            if(!String.IsNullOrEmpty(searchString))
            {
                string[] renterNames;
                renterNames = searchString.Split(' ');
                if (renterNames.Count() == 1)
                {
                    renter = renter.Where(r => r.lastName.Contains(searchString) || r.firstName.Contains(searchString));
                }
                else
                {
                    string r1 = renterNames[0];
                    string r2 = renterNames[1];
                    renter = renter.Where(r => r.firstName.Contains(r1) && r.lastName.Contains(r2));
                }
            }
            var renterList = renter.ToPagedList(pageNumber, pgSize);

            return View(renterList);
        }

        // GET: Renters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter renter = db.renter.Find(id);
            if (renter == null)
            {
                return HttpNotFound();
            }
            return View(renter);
        }

        // GET: Renters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Renters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "renterID,firstName,lastName,phone,email")] Renter renter)
        {
            if (ModelState.IsValid)
            {
                db.renter.Add(renter);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(renter);
        }

        // GET: Renters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter renter = db.renter.Find(id);
            if (renter == null)
            {
                return HttpNotFound();
            }
            return View(renter);
        }

        // POST: Renters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "renterID,firstName,lastName,phone,email")] Renter renter)
        {
            if (ModelState.IsValid)
            {
                db.Entry(renter).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(renter);
        }

        // GET: Renters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Renter renter = db.renter.Find(id);
            if (renter == null)
            {
                return HttpNotFound();
            }
            return View(renter);
        }

        // POST: Renters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Renter renter = db.renter.Find(id);
            db.renter.Remove(renter);
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

        public ActionResult RepeatRenters(int? page)
        {
            int pgSize = 5;
            int pageNumber = (page ?? 1);
            var renters = db.renter.OrderBy(r => r.lastName).ThenBy(r => r.firstName).Where(r => r.leases.Count > 1);
            var renterList = renters.ToPagedList(pageNumber, pgSize);
            return View(renterList);
        }
    }
}
