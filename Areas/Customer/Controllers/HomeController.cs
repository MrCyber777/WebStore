using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Extensions;
using WebStore.Models;
using WebStore.Models.ViewModel;
using WebStore.Utility;

namespace WebStore.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        //private int _pageSize = 3;

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

        [BindProperty]
        public ProductsViewModel productsVM { get; set; }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
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
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);

            // Проверяем, если наш созданный лист равен null, то создаём новый экземпляр
            if (listOfShoppingCart is null)
                listOfShoppingCart = new List<int>();

            // Добавляем полученный из представления ID в массив
            listOfShoppingCart.Add(id);

            // Сериализуем и записываем в сессию лист с ID товаров
            HttpContext.Session.Set(SD.SessionKey, listOfShoppingCart);

            // Переадресовываем пользователя на страницу Index
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(int productPage = 1)
        {
            StringBuilder param = new();
            param.Append("/Customer/Home?productPage=:");

            //var productList = await _db.Products.ToListAsync();
            ProductListViewModel productsVM = new();
            productsVM.Products = await _db.Products.ToListAsync();

            var count = productsVM.Products.Count;
            productsVM.Products = productsVM.Products.OrderBy(x => x.Name)
                                                   .Skip((productPage - 1) * SD.PageSize)
                                                   .Take(SD.PageSize)
                                                   .ToList();

            productsVM.PaginationInfo = new()
            {
                CurrentPage=productPage,
                ItemPerPage= SD.PageSize,
                TotalItems=count,
                UrlParam=param.ToString()
            };


            return View(productsVM);
        }
        public IActionResult Remove(int id)
        {
            // Создать массив типа Лист и записать в него десериализованные данные из сессии
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);

            // Check, if arrays count > 0
            if (listOfShoppingCart.Count > 0)
            {
                // Check, if element contains
                if (listOfShoppingCart.Contains(id))
                    listOfShoppingCart.Remove(id);
            }
            // Update session data
            HttpContext.Session.Set(SD.SessionKey, listOfShoppingCart);

            // Add SM message
            TempData["SM"] = "Product has been removed successfully!";

            // Redirect to index page
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions {Expires=DateTimeOffset.UtcNow.AddYears(1)}
                );

            return LocalRedirect(returnUrl);
        }
    }
}
