using Car_Store.AppDbContext;
using Car_Store.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Store.Controllers
{
    public class AdminController : Controller
    {
        private readonly VroomDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public AdminController(VroomDbContext db, UserManager<IdentityUser> userManager)
        {
           _db = db;
            _userManager = userManager;
        }
        // GET: AdminController
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_db.Users.ToList());
        }

        // GET: AdminController/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        // POST: AdminController/Edit/5
        [HttpPost, ActionName("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> EditUser(EditUserViewModel userView)
        {
            var user = await _userManager.FindByIdAsync(userView.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"UserID :{userView.Id} of this customer is not found.";
                return View("NotFound");
            }
            else
            {
                user.UserName = userView.UserName;
                user.Email = userView.Email;
                user.PhoneNumber = userView.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(userView);
        }

        // GET: AdminController/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }

        // POST: AdminController/Delete/5
        [HttpPost, ActionName("Delete")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index");
            }
        }
    }
}
