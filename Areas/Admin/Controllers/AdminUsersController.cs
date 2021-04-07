using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task <IActionResult> Index()
        {

            return View(await _db.ApplicationUsers.ToListAsync());
        }
        [HttpGet]
        public  IActionResult Edit(string id)
        {
            if (id == null)
                id.Trim();

            return View(id);

            
        }
       [HttpPost]
       [ActionName("Edit")]
       [ValidateAntiForgeryToken]
       public IActionResult EditPost(string id, ApplicationUser applicationUser)
        {
            if (applicationUser.Id == id)
                return View(applicationUser.Id);


            return RedirectToAction(nameof(Index));
                              
            
        }
    }
}
