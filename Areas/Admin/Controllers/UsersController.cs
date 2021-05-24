using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models.ViewModel;
using WebStore.Utility;

namespace WebStore.Areas.Admin.Controllers
{
    //[Authorize(Roles = SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _db;
             
        public UsersController(  ApplicationDbContext db)
        {
            _db = db;
                    
        }
      
        //[HttpGet]
        //public IActionResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[ActionName("Create")]
        //public async Task<IActionResult> CreatePost()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser appUser = new ApplicationUser { Email = CreateUserVM.Email, UserName = CreateUserVM.Email };
        //        var result = await _userManager.CreateAsync(appUser, CreateUserVM.Password);
        //        if (result.Succeeded)
        //            return RedirectToAction("Index");
        //        else
        //        {
        //            foreach (var error in result.Errors)
        //                ModelState.AddModelError(string.Empty, error.Description);
        //        }

        //    }
        //    return View(CreateUserVM);
        //}
        //[HttpGet]
        //public async Task<IActionResult> Edit(string id)
        //{
        //    IdentityUser appUser = await _userManager.FindByIdAsync(id);
        //    if (appUser is null)
        //        return NotFound();
            

        //    EditUserViewModel model = new EditUserViewModel { Id = appUser.Id, Email = appUser.Email };

        //    return View(model);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        IdentityUser appUser = await _userManager.FindByIdAsync(EditUserVM.Id);
        //        if(appUser is not null)
        //        {
        //            appUser.Email = EditUserVM.Email;
        //            appUser.UserName = EditUserVM.Email;

        //            var result = await _userManager.UpdateAsync(appUser);
        //            if (result.Succeeded)
        //                return RedirectToAction("Index");
        //            else
        //            {
        //                foreach (var error in result.Errors)
        //                    ModelState.AddModelError(string.Empty, error.Description);
        //            }
        //        }
        //    }
        //    return View(EditUserVM);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    IdentityUser appUser = await _userManager.FindByIdAsync(id);
        //    if (appUser is not null)
        //    {
        //        IdentityResult result = await _userManager.DeleteAsync(appUser);
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
