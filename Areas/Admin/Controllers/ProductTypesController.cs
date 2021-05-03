using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Utility;

namespace WebStore.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class ProductTypesController : Controller
    {
        // Database dependency injection
        private readonly ApplicationDbContext _db;
       
     
        public ProductTypesController(ApplicationDbContext db) 
        {
            _db = db;
        }
        public IActionResult Index()
        {
           
            //List<ProductTypes> productTypesList = _db.ProductsTypes.ToList(); //  Get all  types of products from the database
            return View(_db.ProductsTypes.ToList());
        }

        //GET:Admin/ProductTypes/Create
        [HttpGet]
        public IActionResult Create()
        {
            //Возвращает представление
            return View();
        }
        //POST:Admin/ProductTypes/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductTypes productType)
        {
            // 1. Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                // 2. Если модель не валидная, возвращаем представление вместе с моделью для исправления ошибок
                return View(productType);
            }

            // 1.1 Если модель валидная, то добавляем значение полей модели в сущности Entity
            _db.Add(productType);
            // 1.2 Сохраняем в базе данных асинхронно
            await _db.SaveChangesAsync();

            // 1.3 Добавляем сообщение о успешном добавлении типа в TempData
            TempData["SM"] = $"Product type: {productType.Name} added successfully ";

            // 1.4 Переадресовываем пользователя на страницу Index
            return RedirectToAction(nameof(Index));

            
        }
        //GET:Admin/ProductTypes/Edit
        [HttpGet]
        public async Task <IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productType =  await _db.ProductsTypes.FindAsync(id);
            
            if (id == null)
                return NotFound();

            return View(productType);
        }
        //POST:Admin/ProductTypes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes productType,int id)
        {
            if (id != productType.Id)           
                return NotFound();
            
            if (!ModelState.IsValid)           
                return View(productType);          
            
            _db.Update(productType);
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Product type: {productType.Name} edited successfully ";
            return RedirectToAction(nameof(Index));           

        }
        [HttpGet]
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null) return NotFound();
            var productType = await _db.ProductsTypes.FindAsync(id);
            if (productType == null) return NotFound();
            return View(productType);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            ProductTypes productType = await _db.ProductsTypes.FindAsync(id);
            if (productType == null) return NotFound();
            return View(productType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeletePost(int? id)
        {
            // Получаем тип продукта из базы и записывааем в переменную
            ProductTypes productTypes = await _db.ProductsTypes.FindAsync(id);

            // Проверяем, найден ли продукт или получен NULL
            if (productTypes != null)
            {
                // Удаляем через сущности Entity
                _db.ProductsTypes.Remove(productTypes);
                // Сохраняем изменения в базе
                await _db.SaveChangesAsync();

                // Добавляем сообщение о успешном удалении
                TempData["SM"] = $"Product type {productTypes.Name}  has been deleted successfully";
            }
            else
                // Добавляем сообщение о провале удаления
                TempData["SM"] = $"Product type  can not be deleted";

            // Переадресовываем на страницу Index
            return RedirectToAction(nameof(Index));
        }
    }
}
