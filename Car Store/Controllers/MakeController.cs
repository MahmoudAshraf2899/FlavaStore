using Car_Store.AppDbContext;
using Car_Store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Car_Store.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class MakeController : Controller
    {
        private readonly VroomDbContext _db;
        public MakeController(VroomDbContext db)
        {
            _db = db;
        }
        // GET: MakeController
        public ActionResult Index()
        {
            return View(_db.Makes.ToList());
        }

        // GET: MakeController/Details/5
        public ActionResult Details(int id)
        {
            var make = _db.Makes.Find(id);
            return View(make);
        }

        // GET: MakeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MakeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Make make)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Add(make);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(make);
            }
            catch
            {
                return View();
            }
        }

        // GET: MakeController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Make make = _db.Makes.Find(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        // POST: MakeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Make make)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _db.Update(make);
                    _db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(make); ;
            }
            catch
            {
                return View();
            }
        }

        // GET: MakeController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Make make = _db.Makes.Find(id);
            if (make == null)
            {
                return NotFound();
            }
            return View(make);
        }

        // POST: MakeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Make make = _db.Makes.Find(id);
                _db.Makes.Remove(make);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
