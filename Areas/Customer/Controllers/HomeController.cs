using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModel;

namespace WebStore.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

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
            var productList = await _db.Products.Include(x => x.ProductTypes)
                                                .Include(x => x.SpecialTags)
                                                .ToListAsync();
            return View(productList);
        }
        [HttpGet]
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null)
                return NotFound();
            productsVM.Products = await _db.Products.Include(x => x.ProductTypes)
                                                    .Include(x => x.SpecialTags)
                                                    .FirstOrDefaultAsync(x=>x.Id==id);
            if (productsVM.Products == null)
                     return NotFound();

            return View(productsVM);
        }
    }
}
