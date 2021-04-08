using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models.ViewModel;
using WebStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using WebStore.Utility;
using Microsoft.AspNetCore.Authorization;

namespace WebStore.Areas.Admin.Controllers
{
    [Authorize(Roles =SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]// Привязывает свойство к Post методам всего контроллера ( не требует передачи через параметр ) 

        public ProductsViewModel productsVM { get; set; }

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            productsVM = new ProductsViewModel()
            {
                Products = new Product(),
                ProductTypes = _db.ProductsTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList()
            };
        }
        public async Task<IActionResult> Index()
        {
            var products = _db.Products.Include(x => x.ProductTypes).Include(x => x.SpecialTags);
            return View(await products.ToListAsync());
        }
        //GET:Admin/Products/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View(productsVM);
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid)
                return View(productsVM);

            // Если модель валидная, добавляем информацию в сущности Entity
            _db.Products.Add(productsVM.Products);

            // Сохраняем изменения в базе данных
            await _db.SaveChangesAsync();

            // Логика сохранения картинок в базу
            // Получаем путь к корневому каталогу сайта
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Получаем все изображения из формы
            var files = HttpContext.Request.Form.Files;

            // Получаем продукт из базы данных
            var productFromDb = _db.Products.Find(productsVM.Products.Id);

            // Проверяем, получен ли картинки из формы
            if (files.Count != 0)
            {
                // Комбинируем путь к каталогу сохранения (e.g. wwwroot/Images/ProductImage)
                var uploadPath = Path.Combine(webRootPath, SD.ImageFolder);

                // Получаем расширение загруженного через форму файла
                var extension = Path.GetExtension(files[0].FileName);

                // Сохраняем изображение в нужном каталоге
                using (var fileStream = new FileStream(Path.Combine(uploadPath, productsVM.Products.Id + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }

                // Записываем в базу данных путь к сохранённому изображению
                productFromDb.Image = $"\\{SD.ImageFolder}\\{productsVM.Products.Id}{extension}";
            }
            else
            {
                // Формируем путь к изображению по умолчанию
                var uploadPath = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);

                // Копируем изображение по умолчанию в папку конкретного продукта
                System.IO.File.Copy(uploadPath, webRootPath + @"\" + SD.ImageFolder + @"\" + productsVM.Products.Id + ".png");

                // Сохраняем путь к изображению в модель
                productFromDb.Image = $"\\{SD.ImageFolder}\\{productsVM.Products.Id}.png";
            }

            // Сохраняем все изменения в базе данных
            await _db.SaveChangesAsync();

            // Добавляем сообщение о успешном добавлении продукта
            TempData["SM"] = $"Product {productsVM.Products.Name} has been added successfully";

            // Переадресовываем пользователя на страницу Index
            return RedirectToAction(nameof(Index));

        }
        //GET: Admin/Products/Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // 1. Проверяем полученный ID на Null
            if (id is null)
                return NotFound();

            // 2. Заполняем ViewModel данными из базы
            productsVM.Products = await _db.Products
                                           .Include(x => x.ProductTypes)
                                           .Include(x => x.SpecialTags)
                                           .FirstOrDefaultAsync(x => x.Id == id);

            // 3. Проверяем, найдены ли данные или получен null
            if (productsVM.Products is null)
                return NotFound();

            // 4. Если данные найдены, возвращаем модель в представление
            return View(productsVM);
        }
        //POST:Admin/Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit()
        {
            if (!ModelState.IsValid)
                return View(productsVM);



            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            Product productFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == productsVM.Products.Id);
            if ((files.Count != 0) && (files[0] != null))
            {
                string uploadPath = Path.Combine(webRootPath, SD.ImageFolder);
                string extension = Path.GetExtension(files[0].FileName);
                string previousExtension = Path.GetExtension(productFromDb.Image);

                if (System.IO.File.Exists(Path.Combine(uploadPath + productsVM.Products.Id + previousExtension)))
                    System.IO.File.Delete(Path.Combine(uploadPath + productsVM.Products.Id + previousExtension));

                using (var fileStream = new FileStream(Path.Combine(uploadPath, productsVM.Products.Id + extension), FileMode.Create))
                {
                    await files[0].CopyToAsync(fileStream);
                }


                productsVM.Products.Image = $"\\{SD.ImageFolder}\\{productsVM.Products.Id}{extension}";
            }
            if (productsVM.Products.Image != null)
                productFromDb.Image = productsVM.Products.Image;

            productFromDb.Name = productsVM.Products.Name;
            productFromDb.Price = productsVM.Products.Price;
            productFromDb.Available = productsVM.Products.Available;
            productFromDb.ShadeColor = productsVM.Products.ShadeColor;
            productFromDb.ProductTypeId = productsVM.Products.ProductTypeId;
            productFromDb.SpecialTagId = productsVM.Products.SpecialTagId;


            await _db.SaveChangesAsync();

            TempData["SM"] = $"Product {productsVM.Products.Name} has been changed successfully";

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            productsVM.Products = await _db.Products
                                         .Include(x => x.ProductTypes)
                                         .Include(x => x.SpecialTags)
                                         .FirstOrDefaultAsync(x => x.Id == id);
            if (productsVM.Products == null)
                return NotFound();

            return View(productsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();
            productsVM.Products = await _db.Products.Include(x => x.ProductTypes)
                                                    .Include(x => x.SpecialTags)
                                                    .FirstOrDefaultAsync(x => x.Id == id);

            if (productsVM.Products == null)
                return NotFound();

            return View(productsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Product productFromDb = await _db.Products.FirstOrDefaultAsync(x => x.Id == productsVM.Products.Id);

            if (productFromDb == null)
                return NotFound();
            else
            {
                string uploadPath = Path.Combine(webRootPath, SD.ImageFolder);
                string extension = Path.GetExtension(productFromDb.Image);
                if (System.IO.File.Exists(uploadPath + productFromDb.Id + extension))
                    System.IO.File.Delete(uploadPath + productFromDb.Id + extension);
                _db.Products.Remove(productFromDb);
            }
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Product{productsVM.Products.Name} has been deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
