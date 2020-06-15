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

namespace ApartmentsRUS.Controllers
{
    public class BulkUploadsController : Controller
    {
        private Context db = new Context();

        // GET: BulkUploads
        public ActionResult Index()
        {
            return View(db.bulkUpload.ToList());
        }

        // GET: BulkUploads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BulkUpload bulkUpload = db.bulkUpload.Find(id);
            if (bulkUpload == null)
            {
                return HttpNotFound();
            }
            return View(bulkUpload);
        }

        // GET: BulkUploads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BulkUploads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            { string prefix = "";
                if (Request.Form["prefix"] != null)
                {
                    prefix = Request.Form["prefix"];
                }
            //    iterating through multiple file collection 
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/BulkUploaded/") + prefix + InputFileName);
                        //Save file to server folder
                        file.SaveAs(ServerSavePath);
                        //assigning file uploaded status to ViewBag for showing message to user.
                        ViewBag.UploadStatus = files.Count().ToString() + " files uploaded successfully.";
                        // now create a new database record and save it
                        BulkUpload bulkUpload = new BulkUpload();
                        bulkUpload.imageUrl = prefix+InputFileName;
                        bulkUpload.dateUploaded = DateTime.Now;
                        bulkUpload.include = false;  // default slide show inclusion is false
                        db.bulkUpload.Add(bulkUpload);
                        db.SaveChanges();
                    }
                }
            }
            return View();
        }

        // GET: BulkUploads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BulkUpload bulkUpload = db.bulkUpload.Find(id);
            if (bulkUpload == null)
            {
                return HttpNotFound();
            }
            return View(bulkUpload);
        }

        // POST: BulkUploads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,fileName,imageUrl,dateUploaded,include")] BulkUpload bulkUpload)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bulkUpload).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bulkUpload);
        }

        // GET: BulkUploads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BulkUpload bulkUpload = db.bulkUpload.Find(id);
            if (bulkUpload == null)
            {
                return HttpNotFound();
            }
            return View(bulkUpload);
        }

        // POST: BulkUploads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BulkUpload bulkUpload = db.bulkUpload.Find(id);
            db.bulkUpload.Remove(bulkUpload);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SlideShow()
        {
            var selectedFiles = db.bulkUpload.Where(s => s.include == true).ToList();
            return View(selectedFiles);
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
