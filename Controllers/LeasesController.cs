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

namespace ApartmentsRUS.Controllers
{
    public class LeasesController : Controller
    {
        private Context db = new Context();

        // GET: Leases
        public ActionResult Index()
        {
            var lease = db.lease.Include(l => l.apartment).Include(l => l.renter);
            return View(lease.ToList());
        }

        // GET: Leases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lease lease = db.lease.Find(id);
            if (lease == null)
            {
                return HttpNotFound();
            }
            return View(lease);
        }

        // GET: Leases/Create
        public ActionResult Create()
        {
            ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentAddr");
            ViewBag.renterID = new SelectList(db.renter, "renterID", "fullName");
            return View();
        }

        // POST: Leases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "leaseID,renterID,apartmentID,startDate,duration,securityDeposit,monthlyRent,depositRefunded,amtRefunded")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                db.lease.Add(lease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentAddr", lease.apartmentID);
            ViewBag.renterID = new SelectList(db.renter, "renterID", "fullName", lease.renterID);
            return View(lease);
        }

        // GET: Leases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lease lease = db.lease.Find(id);
            if (lease == null)
            {
                return HttpNotFound();
            }
            ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentAddr", lease.apartmentID);
            ViewBag.renterID = new SelectList(db.renter, "renterID", "fullName", lease.renterID);
            return View(lease);
        }

        // POST: Leases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "leaseID,renterID,apartmentID,startDate,duration,securityDeposit,monthlyRent,depositRefunded,amtRefunded")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentAddr", lease.apartmentID);
            ViewBag.renterID = new SelectList(db.renter, "renterID", "fullName", lease.renterID);
            return View(lease);
        }

        // GET: Leases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lease lease = db.lease.Find(id);
            if (lease == null)
            {
                return HttpNotFound();
            }
            return View(lease);
        }

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lease lease = db.lease.Find(id);
            db.lease.Remove(lease);
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
