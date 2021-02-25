using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Extensions;
using WebStore.Models;
using WebStore.Models.ViewModel;

namespace WebStore.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public int PageSize = 4;

        [BindProperty]
        public ProductsViewModel productsVM { get; set; }
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
            productsVM = new ProductsViewModel()
            {
                Products = new Product(),
                ProductTypes = _db.ProductsTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList()
            };
        }
        public async Task<IActionResult> Index()
        {
            var productList = await _db.Products.ToListAsync();
            return View(productList);
        }
      
        [HttpGet]
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null)
                return NotFound();
            Product product = await _db.Products.FindAsync(id);
                                                                       
            if (product == null)
                     return NotFound();

            return View(product);
        } 
        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int id)
        {
            // Создать массив типа Лист и записать в него десериализованные данные из сессии
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");

            // Проверяем, если наш созданный лист равен null, то создаём новый экземпляр
            if (listOfShoppingCart is null)
                listOfShoppingCart = new List<int>();

            // Добавляем полученный из представления ID в массив
            listOfShoppingCart.Add(id);

            // Сериализуем и записываем в сессию лист с ID товаров
            HttpContext.Session.Set("sShoppingCart", listOfShoppingCart);

            // Переадресовываем пользователя на страницу Index
            return RedirectToAction(nameof(Index));
        }      
    }
}
