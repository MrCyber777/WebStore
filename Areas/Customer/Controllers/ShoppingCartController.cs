using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Extensions;
using WebStore.Models;
using WebStore.Models.ViewModel;

namespace WebStore.Areas.Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new()
            {
                Products  =  new()
            };
        }
        [HttpGet]
        public async Task <IActionResult> Index(int id)
        {
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");
            if (listOfShoppingCart.Count > 0)
            {
                foreach(var item in listOfShoppingCart)
                {
                   Product product = await _db.Products.Include(x => x.ProductTypes)
                                                       .Include(x => x.SpecialTags)                                                        
                                                       .FirstOrDefaultAsync(x=>x.Id==id);
                   // ShoppingCartVM.Products = product;
                }              
            }
            return View(ShoppingCartVM);
        }
    }
}
