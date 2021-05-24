using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModel;
using WebStore.Utility;

namespace WebStore.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private int _pageSize = 3;

        [BindProperty]// Привязывает свойство к Post методам всего контроллера ( не требует передачи через параметр ) 

        public AdminUsersListViewModel adminUsersVM { get; set; }

        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task <IActionResult> Index(int adminUserPage = 1)
        {
            StringBuilder param = new();
            param.Append("/Admin/AdminUsers?adminUserPage=:");
            AdminUsersListViewModel adminUsersVM = new();
            adminUsersVM.ApplicationUsers = await _db.ApplicationUsers.ToListAsync();
            var count = adminUsersVM.ApplicationUsers.Count;
            adminUsersVM.ApplicationUsers = adminUsersVM.ApplicationUsers.OrderBy(x => x.Name)
                                                                       .Skip((adminUserPage - 1) * _pageSize)
                                                                       .Take(_pageSize)
                                                                       .ToList();
            adminUsersVM.PaginationInfo = new()
            {
                CurrentPage=adminUserPage,
                ItemPerPage=_pageSize,
                TotalItems=count,
                UrlParam=param.ToString()
            };
            

            return View(adminUsersVM/*await _db.ApplicationUsers.ToListAsync()*/);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            //if (id == null || id.Trim().Length == 0)
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            ApplicationUser userFromDB = await _db.ApplicationUsers.FindAsync(id);
            if (userFromDB == null)
                return NotFound();

            return View(userFromDB);

            
        }
       [HttpPost]      
       [ValidateAntiForgeryToken]
       public async Task<IActionResult> Edit(string id, ApplicationUser applicationUser)
        {
            if (applicationUser.Id != id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(applicationUser);

            ApplicationUser userFromDB = await _db.ApplicationUsers.FindAsync(id);
            userFromDB.Name = applicationUser.Name;
            userFromDB.PhoneNumber = applicationUser.PhoneNumber;

            await _db.SaveChangesAsync();          

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            ApplicationUser userFromDB = await _db.ApplicationUsers.FindAsync(id);
            if (userFromDB == null)
                return NotFound();

            return View(userFromDB);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(string id)
        {
            ApplicationUser userFromDB = await _db.ApplicationUsers.FindAsync(id);
            if (userFromDB == null)
                return NotFound();

            userFromDB.LockoutEnd = DateTime.Now.AddYears(1000);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
