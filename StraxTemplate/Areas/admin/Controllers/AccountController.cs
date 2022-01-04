using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StraxTemplate.Data;
using StraxTemplate.Models;
using StraxTemplate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StraxTemplate.Areas.admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(AppDbContext context,UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,RoleManager<IdentityRole> roleManager )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

       
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VmRegister model)
        {
            if(ModelState.IsValid)
            {
                CustomUser user = new CustomUser()
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.Phone,
                    Adress   = model.Adress,
                    Email = model.Email,
                    UserName=model.Email,
                };

               var result= await _userManager.CreateAsync(user, model.Password);
                if(result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Login(VmRegister model)
        {
            if (ModelState.IsValid)
            {


              var result=  await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not valid");
                    return View(model);
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        public IActionResult Users()
        {
            VmUser model = new VmUser()
            {
                CustomUsers = _context.CustomUsers.ToList()
               

            };

            Dictionary<string, string> userRoles = new Dictionary<string, string>();
            foreach (var user in _context.CustomUsers.ToList())
            {
             IdentityUserRole<string> role=  _context.UserRoles.FirstOrDefault(ur=>ur.UserId==user.Id);
                if (role !=null)
                {
                    string roleName = _context.Roles.Find(role.RoleId).Name;
                    userRoles.Add(user.Id, roleName);
                }
            }

            model.UserRoles = userRoles;
            return View(model);
        }

        public IActionResult UsersUpdate(string id)
        {
            CustomUser user = _context.CustomUsers.Find(id);
            IdentityUserRole<string> role = _context.UserRoles.FirstOrDefault(u => u.UserId == user.Id);
            if (role!=null)
            {
                user.RoleId = role.RoleId;
            }
            
            ViewBag.Roles = _context.Roles.ToList();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> UsersUpdate(CustomUser model)
        {

            if(ModelState.IsValid)
            {
                CustomUser customUser = _context.CustomUsers.Find(model.Id);
                customUser.Name = model.Name;
                customUser.Surname = model.Surname;
                customUser.Email = model.Email;
                customUser.PhoneNumber = model.PhoneNumber;
                customUser.Adress = model.Surname;
              
                IdentityUserRole<string> userRole=_context.UserRoles.FirstOrDefault(r=>r.UserId==model.Id);  
                string newRoleName = _context.Roles.Find(model.RoleId).Name;
                if (userRole != null)
                {
                    string oldRoleName = _context.Roles.Find(userRole.RoleId).Name;
                    await   _userManager.RemoveFromRoleAsync(customUser, oldRoleName);
                }

              
            
                
                await _userManager.AddToRoleAsync(customUser, newRoleName);
           
                _context.SaveChanges();
                return RedirectToAction(nameof(Users));
                return RedirectToAction("Users");
            }

            
            ViewBag.Roles = _context.Roles.ToList();
            return View(model);
        }



        public IActionResult Roles()
        {

            return View(_context.Roles.ToList());
        }

        public IActionResult RoleCreate()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RoleCreate(IdentityRole model)
        {
            if(ModelState.IsValid)
            {
                await _roleManager.CreateAsync(model);
                return RedirectToAction("roles");
            }

            return View(model);
        }


        public IActionResult RoleUpdate(string id)
        {

            return View(_context.Roles.Find(id));
        }

        [HttpPost]
        public async Task<IActionResult> RoleUpdate(IdentityRole model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.UpdateAsync(model);
               
                return RedirectToAction("roles");
            }

            return View(model);
        }
    }

}
