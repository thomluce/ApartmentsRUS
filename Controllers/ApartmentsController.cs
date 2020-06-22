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
using System.Net.Mail;
using PagedList;
using System.Web.Services.Description;

namespace ApartmentsRUS.Controllers
{
    [Authorize]
    public class ApartmentsController : Controller
    {
        private Context db = new Context();

        // GET: Apartments
        [AllowAnonymous]
        public ActionResult Index(int? page, string city, string state)
        {
            var apartment = db.apartment.Include(a => a.building);
            apartment = from a in db.apartment select a;

            int pgSize = 15;
            int pageNumber = (page ?? 1);
            ViewBag.city = String.IsNullOrEmpty(city) ? "" : city;
            ViewBag.state = String.IsNullOrEmpty(state) ? "" : state;
            //sort by city and state
            apartment = apartment.OrderBy(a => a.building.state).ThenBy(a => a.building.city).ThenBy(a=>a.building.street).ThenBy(a=>a.apartmentNum);

            if(!string.IsNullOrEmpty(state) && !string.IsNullOrEmpty(city))
            {
                apartment = apartment.Where(a => a.building.state==state).Where(a => a.building.city==city);
            }
            else if (!string.IsNullOrEmpty(state))
            {
                apartment = apartment.Where(a => a.building.state==state);
            }
            else if (!string.IsNullOrEmpty(city))
            {
                apartment = apartment.Where(a => a.building.city == city);
            }


            var apartmentList = apartment.ToPagedList(pageNumber, pgSize);

            return View(apartmentList);

        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.apartment.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddr");
            return View();
        }

        // POST: Apartments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "apartmentID,apartmentNum,buildingID,bedrooms,bathrooms,maxOccupancy")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.apartment.Add(apartment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddr", apartment.buildingID);
            return View(apartment);
        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.apartment.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddress", apartment.buildingID);
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "apartmentID,apartmentNum,buildingID,bedrooms,bathrooms,maxOccupancy")] Apartment apartment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apartment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.buildingID = new SelectList(db.building, "buildingID", "buildingAddr", apartment.buildingID);
            return View(apartment);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.apartment.Find(id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Apartment apartment = db.apartment.Find(id);
            db.apartment.Remove(apartment);
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

        public ActionResult leaseExpiring(string city, string state)
        {
            int leadTime = 3;
            var lease = from a in db.lease select a;

            lease = lease.OrderBy(l => l.apartment.building.state).ThenBy(l => l.apartment.building.city).ThenBy(l => l.apartment.building.street);
            if(!String.IsNullOrEmpty(state) && !String.IsNullOrEmpty(city)) 
            {
                lease = lease.Where(l => l.apartment.building.state == state && l.apartment.building.state == city);
            }
            else if (!String.IsNullOrEmpty(state))
            {
                lease = lease.Where(l => l.apartment.building.state == state);
            }
            else if (!String.IsNullOrEmpty(city))
            {
                lease = lease.Where(l => l.apartment.building.city == city);
            }
           
            int leasesNearExpiring = 0;
            string notification = "Lease expiration notifications sent to:<br/>";
            ViewBag.leaseEnd = DateTime.Now.AddMonths(leadTime);
            foreach (var l in lease)
            {
                DateTime leaseDate = l.startDate;
                int duration = l.duration;
                DateTime leaseEnd = leaseDate.AddMonths(duration);
                if (leaseEnd > DateTime.Now && leaseEnd <= DateTime.Now.AddMonths(leadTime))
                {
                    var firstName = l.renter.firstName;
                    var lastName = l.renter.lastName;
                    var addr = l.apartmentAddr;
                    var email = l.renter.email;
                    leasesNearExpiring++;
                    var msg = "Hi " + firstName + " " + lastName + ",\n\nWe wanted to remind you that your lease on " + addr;
                    msg += " is expiring on " + leaseEnd.ToShortDateString() + ".";
                    msg += "\n\nPlease contact us soon if you are interested in renewing your lease.";
                    msg += "\n\nSincerely\nApartments-R-US Management Team";
                    notification += "<br/>" + l.renter.fullName + " at " + addr + "  -  Expiring on " + leaseEnd.ToShortDateString();
                    MailMessage myMessage = new MailMessage();
                    MailAddress from = new MailAddress("apartmentsRUS@gmail.com", "SysAdmin");
                    myMessage.From = from;
                    myMessage.To.Add(email); 
                    myMessage.Subject = "Lease ending";
                    myMessage.Body = msg;
                    try
                    {
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("GmailUserAcnt", "Password"); 
                        smtp.EnableSsl = true;
                     //   smtp.Send(myMessage);
                        TempData["mailError"] = "";
                    }
                    catch (Exception ex)
                    {
                        // this captures an Exception and allows you to display the message in the View
                        TempData["mailError"] = ex.Message;
                        return View("mailError");
                    }
                }
            }
            ViewBag.notification = notification;
            ViewBag.notificationCnt = leasesNearExpiring;
            return View();
        }
    }
}
