using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Extensions;
using WebStore.Models;
using WebStore.Models.ViewModel;
using WebStore.Utility;

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
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);

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
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);

            ShoppingCartVM.Appointment.AppointmentDay = ShoppingCartVM.Appointment
                                                                    .AppointmentDay
                                                                    .AddHours(ShoppingCartVM.Appointment.AppointmentTime.Hour)
                                                                    .AddMinutes(ShoppingCartVM.Appointment.AppointmentTime.Minute);

            Appointment appointment = ShoppingCartVM.Appointment;

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();

            var appointmentId = appointment.Id;

            foreach (var cartItem in listOfShoppingCart)
            {
                ProductsForAppointment productsForAppointment = new()
                {
                    AppointmentId = appointmentId,
                    ProductId = cartItem
                };

                _db.ProductsForAppointments.Add(productsForAppointment);
            }
            await _db.SaveChangesAsync();

            listOfShoppingCart = new List<int>();
            HttpContext.Session.Set(SD.SessionKey, listOfShoppingCart);

            return RedirectToAction(nameof(AppointmentConfirmation), new { Id = appointmentId});
        }
        [HttpGet]
        public async Task<IActionResult> AppointmentConfirmation(int id)
        {
            ShoppingCartVM.Appointment = await _db.Appointments.FindAsync(id);
            List<ProductsForAppointment> listOfProducts = await _db.ProductsForAppointments.Where(x => x.AppointmentId == id)
                                                                                           .ToListAsync();
            foreach(var item in listOfProducts)
            {
                ShoppingCartVM.Products.Add(await _db.Products.Include(x => x.ProductTypes)
                                                              .Include(x => x.SpecialTags)
                                                              .FirstOrDefaultAsync(x => x.Id == item.ProductId));
            }
            return View(ShoppingCartVM);
        }
    }
}
