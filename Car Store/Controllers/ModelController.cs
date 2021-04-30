using Car_Store.AppDbContext;
using Car_Store.Models;
using Car_Store.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Store.Controllers
{
    public class ModelController : Controller
    {
        private readonly VroomDbContext _db;

        [BindProperty]
        public ModelViewModel ModelVM { get; set; }
        public ModelController(VroomDbContext db)
        {
            _db = db;
            ModelVM = new ModelViewModel()
            {
                Makes = _db.Makes.ToList(),
                Model = new Models.Model()
            };
        }
        // GET: ModelController
        public ActionResult Index()
        {
            var model = _db.Models.Include(m => m.Make);
            return View(model);
        }

        // GET: ModelController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ModelController/Create
        public ActionResult Create()
        {
            return View(ModelVM);
        }

        // POST: ModelController/Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ModelVM);
                }
                _db.Models.Add(ModelVM.Model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelController/Edit/5
        public ActionResult Edit(int id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }

        // POST: ModelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(ModelVM);
                }
                _db.Update(ModelVM.Model);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ModelController/Delete/5
        public ActionResult Delete(int id)
        {
            ModelVM.Model = _db.Models.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (ModelVM.Model == null)
            {
                return NotFound();
            }
            return View(ModelVM);
        }

        // POST: ModelController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult CDelete(int id)
        {
            try
            {
                Model model = _db.Models.Find(id);
                if (model == null)
                {
                    return NotFound();
                }
                _db.Models.Remove(model);
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
