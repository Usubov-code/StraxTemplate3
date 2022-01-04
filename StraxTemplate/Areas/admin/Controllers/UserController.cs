using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StraxTemplate.Data;
using StraxTemplate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StraxTemplate.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View(_context.CustomUsers.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(CustomUser model)
        {
            if(ModelState.IsValid)
            {
                _context.CustomUsers.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Update(int Id)
        {

            return View(_context.CustomUsers.Find(Id));
        }

        [HttpPost]
        public IActionResult Update(CustomUser model)
        {
            if(ModelState.IsValid)
            {
                _context.CustomUsers.Update(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id==null)
            {
                return NotFound();
            }

            CustomUser user = _context.CustomUsers.Find(Id);
            if (user==null)
            {
                return NotFound();
            }
            _context.CustomUsers.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

      
       
    }
}
