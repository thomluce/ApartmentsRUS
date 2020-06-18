using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Antlr.Runtime.Tree;
using ApartmentsRUS.DAL;
using ApartmentsRUS.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace ApartmentsRUS.Controllers
{
    [Authorize]
    public class BuildingsController : Controller
    {
        private Context db = new Context();

        // GET: Buildings
        [AllowAnonymous]
        public ActionResult Index(int? page, string startDate, string stopDate, string minValue)
        {
            int pgSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.startDate = String.IsNullOrEmpty(startDate) ? "" : startDate;
            ViewBag.stopDate = String.IsNullOrEmpty(stopDate) ? "" : stopDate; 
            ViewBag.minValue = String.IsNullOrEmpty(minValue) ? "" : minValue;

            //try to convert parameters
            DateTime startDateSearch = DateTime.MinValue;
            try
            {
                startDateSearch = Convert.ToDateTime(startDate);
            }
            catch
            {
              // no problem if it isn't a valid date since the default is set above
            }
            DateTime endDateSearch = DateTime.MaxValue;
            try
            {
                endDateSearch = Convert.ToDateTime(stopDate);
            }
            catch
            {
                // no problem if it isn't a valid date since the default is set above
            }
            decimal decLowSearch = 0.0m;
            try
            {
                decLowSearch = Convert.ToDecimal(minValue);
            }
            catch
            {
                // no problem if it isn't a valid date since the default is set above
            }

            var building = from b in db.building select b;
            // sort the records
            building = building.OrderBy(b => b.state).ThenBy(b => b.city);

            if (!String.IsNullOrEmpty(startDate))
            {
                building = building.Where(b => b.inspectionDate >= startDateSearch);
            }
            if (!String.IsNullOrEmpty(stopDate))
            {
                building = building.Where(b => b.inspectionDate <= endDateSearch);
            }
            if (!String.IsNullOrEmpty(minValue))
            {
                building = building.Where(b => b.appraisedValue >= decLowSearch);
            }
            var buildingList = building.ToPagedList(pageNumber, pgSize);
            return View(buildingList);
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
        public ActionResult Create([Bind(Include = "buildingID,street,city,state,zip,inspectionDate,inspectionResults,appraisedValue,propertyTaxRate,buildingImage")] Building building)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["UploadedImage"];
                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".jpg" && fi.Extension != ".gif")
                    {
                        ViewBag.Errormsg = "The file, " + file.FileName + ", does not have a valid image extension.";

                        return View(building);
                    }
                    else
                    {

                        building.buildingImage = Guid.NewGuid().ToString() + fi.Extension; // create a random identifier
                        file.SaveAs(Server.MapPath("~/Uploads/" + building.buildingImage));
                    }
                }
                db.building.Add(building);
                db.SaveChanges();
                return RedirectToAction("Index", "Buildings");
            }
            else
            {
                return View(building);
            }
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
                    Building currentBuilding = db.building.Find(id);
                    TempData["oldPhoto"] = currentBuilding.buildingImage; // save the current image info 

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
        public ActionResult Edit([Bind(Include = "buildingID,street,city,state,zip,inspectionDate,inspectionResults,appraisedValue,propertyTaxRate,buildingImage")] Building building)
        {
            if (ModelState.IsValid)
            {
                string removeImage = "";
                if(Request.Form["removeImage"] != null)
                {
                    removeImage = Request.Form["removeImage"];
                }
                HttpPostedFileBase file = Request.Files["UploadedImage"];

                if (file != null && file.FileName != null && file.FileName != "")
                {
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fi.Extension != ".png" && fi.Extension != ".jpg" && fi.Extension != "gif")
                    {
                        ViewBag.Errormsg = "Image File Extension is not valid";
                        return View(building);
                    }
                    else
                    {
                        if (TempData["oldPhoto"] != null)
                        {
                            string path = Server.MapPath("~/Uploads/" + TempData["oldPhoto"].ToString());
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
                            building.buildingImage = Guid.NewGuid().ToString() + fi.Extension;
                            file.SaveAs(Server.MapPath("~/Uploads/" + building.buildingImage));
                        }
                    }
                }
                else
                {
                    // no file was selected, so set the photo field back to its original value
                    if (TempData["oldPhoto"] != null)
                    {
                        if (removeImage=="Remove")
                        {
                            building.buildingImage = "";
                            // and remove the physical file
                            string path = Server.MapPath("~/Uploads/" + TempData["oldPhoto"].ToString());
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
                            building.buildingImage = TempData["oldPhoto"].ToString();
                        }
                    }
                }
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

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Building building = db.building.Find(id);
            string imageName = building.buildingImage;
            string path = Server.MapPath("~/Uploads/" + imageName);
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
            db.building.Remove(building);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SlideShow ()
        {

            return View(db.building.ToList());
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
