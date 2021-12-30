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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.Categories.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
          if(ModelState.IsValid)
            {
                _context.Categories.Add(model);
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
           
            return View(_context.Categories.Find(Id));
        }
        [HttpPost]

        public IActionResult Update (Category model)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete (int? Id)
        {
            if(Id==null)
            {
                return NotFound();
            }

            Category category = _context.Categories.Find(Id);

            if(category==null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();


            return RedirectToAction("Index");
        }


    }
}
