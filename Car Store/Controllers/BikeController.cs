using Car_Store.AppDbContext;
using Car_Store.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;
using System.IO;
using Car_Store.Models;
using cloudscribe.Pagination.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Car_Store.Controllers
{
    [Authorize(Roles = "Admin,Executive")]
    public class BikeController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        [BindProperty]
        public BikeViewModel BikeVM { get; set; }
        public BikeController(VroomDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
            BikeVM = new BikeViewModel()
            {
                Makes = _db.Makes.ToList(),
                Models = _db.Models.ToList(),
                Bike = new Models.Bike()
            };
        }
        // GET: ModelController
        public ActionResult Index(string searchEngine, string sortOrder, int pageNumber = 1, int pageSize = 3)
        {
            ViewBag.CurrentSortOrder = sortOrder;
            ViewBag.CurrentSearch = searchEngine;
            ViewBag.PriceSortParam = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            int ExcludeRecords = (pageNumber * pageSize) - pageSize;
            var Bikes = from b in _db.Bikes.Include(m => m.Make).Include(m => m.Model)
                        select b;
            var BikeCount = Bikes.Count();
            if (!String.IsNullOrEmpty(searchEngine))
            {
                Bikes = Bikes.Where(b => b.Make.Name.Contains(searchEngine));
                BikeCount = Bikes.Count();
            }
            //Sorting Logic
            switch (sortOrder)
            {
                case "price_desc":
                    Bikes = Bikes.OrderByDescending(b => b.Price);
                    break;
                default:
                    Bikes = Bikes.OrderBy(b => b.Price);
                    break;
            }
            Bikes = Bikes
            .Skip(ExcludeRecords)
            .Take(pageSize);
            var result = new PagedResult<Bike>
            {
                Data = Bikes.AsNoTracking().ToList(),
                TotalItems = _db.Bikes.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }
        // GET: ModelController/Create
        public ActionResult Create()
        {
            return View(BikeVM);
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
                    BikeVM.Makes = _db.Makes.ToList();
                    BikeVM.Models = _db.Models.ToList();
                    return View(BikeVM);
                }
                _db.Bikes.Add(BikeVM.Bike);
                _db.SaveChanges();
                //Get Bike ID
                var BikeID = BikeVM.Bike.Id;
                //Get WWWRootPath To Save File On Server
                string wwwRootPath = _hostEnvironment.WebRootPath;
                //Get The UploadFiles
                var files = HttpContext.Request.Form.Files;
                //Get The Reference of DBSet for the Bike We Just Have Saved In DB
                var SavedBike = _db.Bikes.Find(BikeID);
                //Upload File On Server And Save Image Path
                if (files.Count != 0)
                {
                    var ImagePath = @"images\bike\";
                    var Extensions = Path.GetExtension(files[0].FileName);
                    var RelativeImagePath = ImagePath + BikeID + Extensions;
                    var AbsImagePath = Path.Combine(wwwRootPath, RelativeImagePath);
                    //Upload File On The Server
                    using (var fileStrem = new FileStream(AbsImagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStrem);
                    }
                    //Set The ImagePathTO Data base
                    SavedBike.ImagePath = RelativeImagePath;
                    _db.SaveChanges();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        // GET: ModelController/Edit/5
        public IActionResult Edit(int id)
        {
            BikeVM.Bike = _db.Bikes.SingleOrDefault(b => b.Id == id);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }
        // POST: ModelController/Create
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    BikeVM.Makes = _db.Makes.ToList();
                    BikeVM.Models = _db.Models.ToList();
                    return View(BikeVM);
                }
                _db.Bikes.Update(BikeVM.Bike);
                _db.SaveChanges();
                //Get Bike ID
                var BikeID = BikeVM.Bike.Id;
                //Get WWWRootPath To Save File On Server
                string wwwRootPath = _hostEnvironment.WebRootPath;
                //Get The UploadFiles
                var files = HttpContext.Request.Form.Files;
                //Get The Reference of DBSet for the Bike We Just Have Saved In DB
                var SavedBike = _db.Bikes.Find(BikeID);
                //Upload File On Server And Save Image Path
                if (files.Count != 0)
                {
                    var ImagePath = @"images\bike\";
                    var Extensions = Path.GetExtension(files[0].FileName);
                    var RelativeImagePath = ImagePath + BikeID + Extensions;
                    var AbsImagePath = Path.Combine(wwwRootPath, RelativeImagePath);
                    //Upload File On The Server
                    using (var fileStrem = new FileStream(AbsImagePath, FileMode.Create))
                    {
                        files[0].CopyTo(fileStrem);
                    }
                    //Set The ImagePathTO Data base
                    SavedBike.ImagePath = RelativeImagePath;
                    _db.SaveChanges();
                }
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
            BikeVM.Bike = _db.Bikes.Include(m => m.Make).SingleOrDefault(m => m.Id == id);
            if (BikeVM.Bike == null)
            {
                return NotFound();
            }
            return View(BikeVM);
        }
        // POST: ModelController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult CDelete(int id)
        {
            try
            {
                Bike bike = _db.Bikes.Find(id);
                if (bike == null)
                {
                    return NotFound();
                }
                _db.Bikes.Remove(bike);
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
