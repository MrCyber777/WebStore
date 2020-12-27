using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Areas.Admin.Controllers
{
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
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var id =  _db.ProductsTypes.Find(Id);            
            if (id == null)
            {
                return NotFound();
            }
            return View(id);
        }
        //POST:Admin/ProductTypes/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(ProductTypes productType,int id)
        {
            if (id != productType.Id)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(productType);          
            }
            _db.Update(productType);
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Product type: {productType.Id} edited successfully ";
            return RedirectToAction(nameof(Index));           

        }
    }
}
