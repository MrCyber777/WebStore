using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModel;
using WebStore.Utility;

namespace WebStore.Areas.Admin
{
    [Area(nameof(Admin))]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public AppointmentViewModel appointmentVM { get; set; } = new();


        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task <IActionResult> Index(string searchKey)
        {
            // 1. Получаем объект типа ClaimsPrincipal
            var currentUser = this.User;

            // 2. Получаем объект идентификации юзера
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            // 3. Получаем доступ к объекту пользователя
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if  (searchKey is null)
                appointmentVM.Appointments = await _db.Appointments.Include(x => x.SalesPerson).ToListAsync();
            else
            {
                appointmentVM.Appointments = await _db.Appointments.Include(x => x.SalesPerson)
                                                                   .Where(x => x.CustomerName.ToLower().Contains(searchKey.ToLower()) ||
                                                                          x.CustomerEmail.Contains(searchKey.ToLower()) ||
                                                                          x.CustomerPhoneNumber.Contains(searchKey.ToLower())).ToListAsync();
            }

            if (User.IsInRole(SD.AdminEndUser))
                appointmentVM.Appointments = appointmentVM.Appointments.Where(x => x.SalesPersonID == claim.Value).ToList();


            return View(appointmentVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var products = (from p in _db.Products
                            join a in _db.ProductsForAppointments
                            on p.Id equals a.ProductId
                            where a.AppointmentId == id
                            select p).Include("ProductTypes") as IEnumerable<Product>;

            AppointmentDetailsViewModel detailsVM = new()
            {
                Appointment = await _db.Appointments.Include(x => x.SalesPerson).FirstOrDefaultAsync(x => x.Id == id),
                SalesPersons = await _db.ApplicationUsers.ToListAsync(),
                Products = products.ToList()
                                                     
                                                    
            };

            return View(detailsVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Appointment appointment,int id)
        {
            if (id != appointment.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(appointment);

            Appointment appointmentFromDB = await _db.Appointments.FindAsync(id);
            appointmentFromDB.AppointmentDay = appointment.AppointmentDay;
            appointmentFromDB.AppointmentTime = appointment.AppointmentTime;
            appointmentFromDB.CustomerEmail = appointment.CustomerEmail;
            appointmentFromDB.CustomerName = appointment.CustomerName;
            appointmentFromDB.CustomerPhoneNumber = appointment.CustomerPhoneNumber;
            appointmentFromDB.IsConfirmed = appointment.IsConfirmed;

            await _db.SaveChangesAsync();
            TempData["SM"] = $"Appointment  has been edited successfully";

            return RedirectToAction(nameof(Index));
        }
      
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            Appointment appointment = await _db.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
            
        }
        [HttpGet]
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null)
                return NotFound();
            Appointment appointmentFromDB = await _db.Appointments.FindAsync(id);
            if (appointmentFromDB == null)
                return NotFound();

            return View(appointmentFromDB);
                
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeletePost(int? id)
        {
            Appointment appointmentFromDB = await _db.Appointments.FindAsync(id);
            if (appointmentFromDB != null)
            {
                _db.Appointments.Remove(appointmentFromDB);
                await _db.SaveChangesAsync();
                TempData["SM"] = $"Appointment  has been deleted successfully";
            }
           
            else            
                TempData["SM"] = $"Appointment can not be deleted";
                return RedirectToAction(nameof(Index));            
        }
    }
}
