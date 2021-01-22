using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models.ViewModel;
using WebStore.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace WebStore.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]// Привязывает свойство к Post методам всего контроллера ( не требует передачи через параметр ) 
        
        public ProductsViewModel productsVM { get; set; }

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;

            productsVM = new ProductsViewModel()
            {
                Products = new Product(),
                ProductTypes = _db.ProductsTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList()
            };
        }
        public async Task <IActionResult> Index()
        {
            var products =  _db.Products.Include(x => x.ProductTypes).Include(x => x.SpecialTags);
            return View(await products.ToListAsync());
        }
    }
}
