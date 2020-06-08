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
    [Authorize]
    public class InvestorsController : Controller
    {
        private Context db = new Context();

        // GET: Investors
        public ActionResult Index()
        {
            var investor = db.investor.Include(i => i.building).Include(i => i.owner);
            return View(investor.ToList());
        }

        // GET: Investors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investor investor = db.investor.Find(id);
            if (investor == null)
            {
                return HttpNotFound();
            }
            return View(investor);
        }

        // GET: Investors/Create
        public ActionResult Create()
        {
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddress");
            ViewBag.ownerID = new SelectList(db.owner, "ownerID", "fullName");
            return View();
        }

        // POST: Investors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "investorID,ownerID,buildingID,purchaseDate,purchasePrice,percentOwnership,saleDate,salePrice")] Investor investor)
        {
            if (ModelState.IsValid)
            {
                db.investor.Add(investor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddress", investor.buildingID);
            ViewBag.ownerID = new SelectList(db.owner, "ownerID", "fullName", investor.ownerID);
            return View(investor);
        }

        // GET: Investors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investor investor = db.investor.Find(id);
            if (investor == null)
            {
                return HttpNotFound();
            }
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddress", investor.buildingID);
            ViewBag.ownerID = new SelectList(db.owner, "ownerID", "fullName", investor.ownerID);
            return View(investor);
        }

        // POST: Investors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "investorID,ownerID,buildingID,purchaseDate,purchasePrice,percentOwnership,saleDate,salePrice")] Investor investor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(investor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddress", investor.buildingID);
            ViewBag.ownerID = new SelectList(db.owner, "ownerID", "fullName", investor.ownerID);
            return View(investor);
        }

        // GET: Investors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Investor investor = db.investor.Find(id);
            if (investor == null)
            {
                return HttpNotFound();
            }
            return View(investor);
        }

        // POST: Investors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Investor investor = db.investor.Find(id);
            db.investor.Remove(investor);
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
