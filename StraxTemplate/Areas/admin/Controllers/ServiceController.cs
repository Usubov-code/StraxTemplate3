using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StraxTemplate.Data;
using StraxTemplate.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StraxTemplate.Areas.admin.Controllers
{
    [Area("admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ServiceController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {

            return View(_context.Services.Include(e=>e.CustomUsers).Include(a=>a.Category).Include(d=>d.DesingType).ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {


            return View();
        }
        [HttpPost]
        public IActionResult Create(Service model)
        {
            if (ModelState.IsValid)
            {
                if (model.DesingType.ImageFile.ContentType == "image/jpeg"|| model.DesingType.ImageFile.ContentType == "image/png")
                {
                    if (model.DesingType.ImageFile.Length<=3145728)
                    {
                        string fileName =Guid.NewGuid()+""+model.DesingType.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath,"Uploads",fileName);

                        using (var stream = new FileStream(filePath,FileMode.Create)) 
                        {
                            model.DesingType.ImageFile.CopyTo(stream);
                        }

                        model.DesingType.Img = fileName;
                        model.CreatedTime = DateTime.Now;
                   

                        _context.Services.Add(model);
                        _context.SaveChanges();
                        
                            return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "you can only upload image file Size 3MB!");
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "you can only upload image file!");
                    return View(model);
                }

                _context.Services.Add(model);
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


            return View(_context.Services.Find(Id));
        }


        [HttpPost]
        public IActionResult Update(Service model)
        {
            if(ModelState.IsValid)
            {
                _context.Services.Update(model);
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

            Service service = _context.Services.Find(Id);
            if(service==null)
            {
                return NotFound();
            }

            _context.Services.Remove(service);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
