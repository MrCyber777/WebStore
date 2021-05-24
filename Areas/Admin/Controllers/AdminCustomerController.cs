using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Utility;

namespace WebStore.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles=SD.SuperAdminEndUser)]
    public class AdminCustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminCustomerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userList = await _db.ClientUsers.ToListAsync();
            TempData["SM"] = "User has been banned successfully!";
                          
            return View(userList);
        }
        public async Task<IActionResult> BanByIp(string id)
        {
            var userFromDB = await _db.ClientUsers.FindAsync(id);
            if (userFromDB is null)
                return NotFound();

            userFromDB.LockoutEnd = DateTime.Now.AddYears(1000);
            IPBlackList iPBlackList = new()
            {
                Address = userFromDB.IP,
                UserId = userFromDB.Id
            };
            await _db.IPBlackLists.AddAsync(iPBlackList);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        public async Task<IActionResult> BanByMac(string id)
        {
            var userFromDB = await _db.ClientUsers.FindAsync(id);
            if (userFromDB is null)
                return NotFound();
            userFromDB.LockoutEnd = DateTime.Now.AddYears(1000);
            MacBlackList macBlackList = new()
            {
                Address = userFromDB.UserMac,
                UserId = userFromDB.Id
            };
            await _db.MacBlackLists.AddAsync(macBlackList);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
