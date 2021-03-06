﻿using System;
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
using PagedList;

namespace ApartmentsRUS.Controllers
{
    [Authorize]
    public class OwnersController : Controller
    {
        private Context db = new Context();

        // GET: Owners
        public ActionResult Index(int? page)
        {
            page = page ?? 1;
            var pgSize = 15;
            ViewBag.page = page;
            ViewBag.pgSize = pgSize;
            var owners = db.owner.OrderBy(o => o.lastName).ThenBy(o => o.firstName);
            var ownerCnt = owners.Count();
            var totalPgs =(ownerCnt+pgSize-1) / pgSize;
            ViewBag.totalPgs = totalPgs;
            var ownerList = owners.Take(pgSize);
            int pg = (int)page;
            if (pg>1)
            {
                int skipAmt = (pg - 1) * pgSize;
                ownerList = owners.Skip(skipAmt).Take(pgSize);
            }
            return View(ownerList.ToList());
        }

        // GET: Owners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owner.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // GET: Owners/Create
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
                    return View("RestrictedOperation");
                }
            }
            else
            {
                return View("MissingRegistration");
            }
        }

        // POST: Owners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ownerID,firstName,lastName,email,phone,taxID")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.owner.Add(owner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(owner);
        }

        // GET: Owners/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owner.Find(id);
            if (owner == null)
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
                    return View(owner);
                }
                else
                {
                    return View("RestrictedOperation");
                }
            }
            else
            {
                return View("MissingRegistration");
            }
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ownerID,firstName,lastName,email,phone,taxID")] Owner owner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(owner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(owner);
        }

        // GET: Owners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Owner owner = db.owner.Find(id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Owner owner = db.owner.Find(id);
            db.owner.Remove(owner);
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

        public ActionResult OwnersAndBuildings()
        {
            var ownerList = db.owner.Include(o => o.investor).ToList();

            return View(ownerList);
        }

        public ActionResult OwnersAndBuildingsPartial()
        {
            var ownerList = db.owner.ToList();

            return View(ownerList);
        }
        public ActionResult RentIncome(int? page)
        {
            int pgSize = 5;
            int pageNumber = (page ?? 1);
            // we need to go from owner->investor->buildings->apartments->leases
            // but it can't all be done at once
            var ownerList = db.owner.OrderBy(o=>o.lastName).ThenBy(o=>o.firstName).ToList();
            var pagedOwnerList = ownerList.ToPagedList(pageNumber, pgSize);
            return View(pagedOwnerList);
        }

    }
}
