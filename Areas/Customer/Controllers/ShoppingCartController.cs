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
    [Area(nameof(Customer))]
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
        public async Task <IActionResult> Index()
        {
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");

            if (listOfShoppingCart.Count > 0)
            {
                foreach(var cartItem in listOfShoppingCart)
                {
                   Product product = await _db.Products.Include(x => x.ProductTypes)
                                                       .Include(x => x.SpecialTags)                                                        
                                                       .FirstOrDefaultAsync(x=>x.Id == cartItem);

                   ShoppingCartVM.Products.Add(product);
                }              
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>("sShoppingCart");     
            
            ShoppingCartVM.Appointment.AppointmentDay=ShoppingCartVM.Appointment.AppointmentDay.AddHours(0.00).AddMinutes(0.00);
            Appointment appointment = ShoppingCartVM.Appointment;

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();
            
            var id = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == ShoppingCartVM.Appointment.Id);

            foreach(var cartItem in listOfShoppingCart)
            {
                ProductsForAppointment productsForAppointment = new();
                productsForAppointment.Products = await _db.Products.Include(x => x.ProductTypes)
                                                                    .Include(x => x.SpecialTags)
                                                                    .FirstOrDefaultAsync(x => x.Id == cartItem);

                productsForAppointment.Appointments = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == cartItem);

                _db.ProductsForAppointments.Add(productsForAppointment);
            }
            await _db.SaveChangesAsync();
            List<int> listOfProducts = new List<int>();
            listOfShoppingCart = listOfProducts;

            return RedirectToAction(nameof(Index));
        }
    }
}
