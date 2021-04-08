using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Utility;

namespace WebStore.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class SpecialTagController : Controller
    {
        //Database dependency injection
        private readonly ApplicationDbContext _db;
        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //POST:Admin/SpecialTag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(SpecialTag specialTag)
        {
            //Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                //Если модель не валидная, возвращаем представление вместе с моделью для исправления ошибок
                return View(specialTag);
            }
            //Если модель валидная-добавляем значение значение полей модели в сущности Entity
            _db.Add(specialTag);
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Special tag:{specialTag.Name} added successfully";
            return RedirectToAction(nameof(Index));
        }
        //GET:Admin/SpecialTag/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            SpecialTag specialTag = await _db.SpecialTags.FindAsync(id);
            if (id == null) return NotFound();
            return View(specialTag);
        }
        //POST:Admin/SpecialTag/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag,int id)
        {
            if (id != specialTag.Id) 
                return NotFound();

            if (!ModelState.IsValid) 
                return View(specialTag);

            _db.Update(specialTag);
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Special tag type: {specialTag.Name} edited successfully ";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            return NotFound();
            SpecialTag specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null)
                return NotFound();
            return View(specialTag);

        }
        [HttpGet]
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null)
                return NotFound();
            SpecialTag specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag == null) return NotFound();
            return View(specialTag);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            SpecialTag specialTag = await _db.SpecialTags.FindAsync(id);
            if (specialTag != null)
            {
                _db.SpecialTags.Remove(specialTag);
                await _db.SaveChangesAsync();
                TempData["SM"] = $"Special tag type {specialTag.Name}  has been deleted successfully";
            }
            else
                TempData["SM"] = $"Special tag type  can not be deleted";
            return RedirectToAction(nameof(Index));
        }
    }
}
