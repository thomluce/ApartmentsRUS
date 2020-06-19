using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApartmentsRUS.DAL;
using ApartmentsRUS.Models;
using Microsoft.AspNet.Identity;

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
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            if (loggedInUser != null) // need to check if we found a user
            {
                bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
                bool isOwner = loggedInUser.role == RegisteredUser.roles.owner;
                if (isAdmin || isOwner)
                {
                    ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentAddr");
                    ViewBag.renterID = new SelectList(db.renter, "renterID", "fullName");
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

        // POST: Leases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "leaseID,renterID,apartmentID,startDate,duration,securityDeposit,monthlyRent,depositRefunded,amtRefunded,leaseDoc")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase file = Request.Files["UploadedDocument"];
                    if (file != null && file.FileName != null && file.FileName != "")
                    {
                        FileInfo fi = new FileInfo(file.FileName);
                        if (fi.Extension != ".doc" && fi.Extension != ".docx" && fi.Extension != ".pdf")
                        {
                            ViewBag.Errormsg = "The file, " + file.FileName + ", does not have a valid document extension.";

                            return View(lease);
                        }
                        else
                        {

                            lease.leaseDoc = Guid.NewGuid().ToString() + fi.Extension; // create a random identifier
                            file.SaveAs(Server.MapPath("~/LeaseDocuments/" + lease.leaseDoc));
                        }
                    }
                    db.lease.Add(lease);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Leases");
                }
                else
                {
                    return View(lease);
                }

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
            Guid memberId;
            Guid.TryParse(User.Identity.GetUserId(), out memberId);
            RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
            if (loggedInUser != null) // need to check if we found a user
            {
                bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
                bool isOwner = loggedInUser.role == RegisteredUser.roles.owner;
                if (isAdmin || isOwner)
                {
                    Lease currentLease = db.lease.Find(id);
                    TempData["leaseDoc"] = currentLease.leaseDoc; // save the current image info 
                    ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentNum", lease.apartmentID);
                    ViewBag.renterID = new SelectList(db.renter, "renterID", "firstName", lease.renterID);
                    return View(lease);
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

        // POST: Leases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "leaseID,renterID,apartmentID,startDate,duration,securityDeposit,monthlyRent,depositRefunded,amtRefunded,leaseDoc")] Lease lease)
        {
            if (ModelState.IsValid)
            {
                string removeDoc = "";
                if (Request.Form["removeDoc"] != null)
                {
                    removeDoc = Request.Form["removeDoc"];
                }

                HttpPostedFileBase file = Request.Files["UploadedDocument"];

                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".doc" && fi.Extension != ".docx" && fi.Extension != "pdf")
                    {
                        ViewBag.Errormsg = "Document File Extension is not valid";
                        return View(lease);
                    }
                    else
                    {
                        if (TempData["leaseDoc"] != null)
                        {
                            string path = Server.MapPath("~/LeaseDocuments/" + TempData["leaseDoc"].ToString());
                            try
                            {
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    // must already be deleted
                                }
                            }
                            catch (Exception Ex)
                            {
                                ViewBag.deleteFailed = Ex.Message;
                                return View("DeleteFailed");
                            }
                        }
                        if (fi.Name != null && fi.Name != "") // i.e., there was a file selected
                        {
                            lease.leaseDoc = Guid.NewGuid().ToString() + fi.Extension;
                            file.SaveAs(Server.MapPath("~/LeaseDocuments/" + lease.leaseDoc));
                        }
                    }
                }
                else
                {
                    // no file was selected, so set the photo field back to its original value 
                    if (TempData["leaseDoc"] != null)
                    {

                        if (removeDoc == "Remove")
                        {
                            lease.leaseDoc = "";
                            string path = Server.MapPath("~/LeaseDocuments/" + TempData["leaseDoc"].ToString());
                            try
                            {
                                if (System.IO.File.Exists(path))
                                {
                                    System.IO.File.Delete(path);
                                }
                                else
                                {
                                    // must already be deleted
                                }
                            }
                            catch (Exception Ex)
                            {
                                ViewBag.deleteFailed = Ex.Message;
                                return View("DeleteFailed");
                            }
                        }
                        else
                        {
                            lease.leaseDoc = TempData["leaseDoc"].ToString();
                        }
                    }
                    db.Entry(lease).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

                ViewBag.apartmentID = new SelectList(db.apartment, "apartmentID", "apartmentNum", lease.apartmentID);
                ViewBag.renterID = new SelectList(db.renter, "renterID", "firstName", lease.renterID);
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
                Guid memberId;
                Guid.TryParse(User.Identity.GetUserId(), out memberId);
                RegisteredUser loggedInUser = db.registeredUsers.Find(memberId);
                if (loggedInUser != null) // need to check if we found a user
                {
                    bool isAdmin = loggedInUser.role == RegisteredUser.roles.admin;
                    bool isOwner = loggedInUser.role == RegisteredUser.roles.owner;
                    if (isAdmin || isOwner)
                    {
                        return View(lease);
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

        // POST: Leases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lease lease = db.lease.Find(id);
            string docName = lease.leaseDoc;
            string path = Server.MapPath("~/LeaseDocuments/" + docName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                else
                {
                    // no file was found, no error
                }
            }
            catch (Exception Ex)
            {
                ViewBag.deleteFailed = Ex.Message;
                return View("DeleteFailed");
            }
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

        public ActionResult leaseExpiring()
        {
            return RedirectToAction("leaseExpiring", "Apartments");
        }
    }
}
