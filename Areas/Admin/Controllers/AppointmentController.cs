using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public AppointmentViewModel appointmentVM { get; set; } = new();
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var details = await GetDetailsAsync(id);
            //var products = (from p in _db.Products
            //                join a in _db.ProductsForAppointments
            //                on p.Id equals a.ProductId
            //                where a.AppointmentId == id
            //                select p).Include("ProductTypes") as IEnumerable<Product>;

            //AppointmentDetailsViewModel detailsVM = new()
            //{
            //    Appointment = await _db.Appointments.Include(x => x.SalesPerson).FirstOrDefaultAsync(x => x.Id == id),
            //    SalesPersons = await _db.ApplicationUsers.ToListAsync(),
            //    Products = products.ToList()
            //};
            //detailsVM.Appointment.AppointmentTime = detailsVM.Appointment.AppointmentTime
            //                                       .AddHours(detailsVM.Appointment.AppointmentDay.Hour)
            //                                       .AddMinutes(detailsVM.Appointment.AppointmentDay.Minute);

            return View(details);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
                return NotFound();

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

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();
            var details = await GetDetailsAsync(id);

            //var products = (from p in _db.Products
            //                join a in _db.ProductsForAppointments
            //                on p.Id equals a.ProductId
            //                where a.AppointmentId == id
            //                select p).Include("ProductTypes") as IEnumerable<Product>;

            //AppointmentDetailsViewModel detailsVM = new()
            //{
            //    Appointment = await _db.Appointments.Include(x => x.SalesPerson).FirstOrDefaultAsync(x => x.Id == id),
            //    SalesPersons = await _db.ApplicationUsers.ToListAsync(),
            //    Products = products.ToList()
            //};
            //detailsVM.Appointment.AppointmentTime = detailsVM.Appointment.AppointmentTime
            //                                       .AddHours(detailsVM.Appointment.AppointmentDay.Hour)
            //                                       .AddMinutes(detailsVM.Appointment.AppointmentDay.Minute);

            return View(details);
        }
        private async Task<AppointmentDetailsViewModel> GetDetailsAsync(int? id)
        {
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
            detailsVM.Appointment.AppointmentTime = detailsVM.Appointment.AppointmentTime
                                                   .AddHours(detailsVM.Appointment.AppointmentDay.Hour)
                                                   .AddMinutes(detailsVM.Appointment.AppointmentDay.Minute);
            return detailsVM;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var details = await GetDetailsAsync(id);

            //var products = (from p in _db.Products
            //                join a in _db.ProductsForAppointments
            //                on p.Id equals a.ProductId
            //                where a.AppointmentId == id
            //                select p).Include("ProductTypes") as IEnumerable<Product>;

            //AppointmentDetailsViewModel detailsVM = new()
            //{
            //    Appointment = await _db.Appointments.Include(x => x.SalesPerson).FirstOrDefaultAsync(x => x.Id == id),
            //    SalesPersons = await _db.ApplicationUsers.ToListAsync(),
            //    Products = products.ToList()
            //};
            //detailsVM.Appointment.AppointmentTime = detailsVM.Appointment.AppointmentTime
            //                                       .AddHours(detailsVM.Appointment.AppointmentDay.Hour)
            //                                       .AddMinutes(detailsVM.Appointment.AppointmentDay.Minute);

            return View(details);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentDetailsViewModel detailsVM, int id)
        {
            if (id != detailsVM.Appointment.Id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(detailsVM);
            detailsVM.Appointment.AppointmentDay = detailsVM.Appointment.AppointmentDay
                                                            .AddHours(detailsVM.Appointment.AppointmentTime.Hour)
                                                            .AddMinutes(detailsVM.Appointment.AppointmentTime.Minute);

            var appointmentFromDB = await _db.Appointments.FindAsync(id);

            appointmentFromDB.CustomerName = detailsVM.Appointment.CustomerName;
            appointmentFromDB.CustomerEmail = detailsVM.Appointment.CustomerEmail;
            appointmentFromDB.CustomerPhoneNumber = detailsVM.Appointment.CustomerPhoneNumber;
            appointmentFromDB.AppointmentTime = detailsVM.Appointment.AppointmentTime;
            appointmentFromDB.AppointmentDay = detailsVM.Appointment.AppointmentDay;
            appointmentFromDB.IsConfirmed = detailsVM.Appointment.IsConfirmed;
            appointmentFromDB.CustomerSurname = detailsVM.Appointment.CustomerSurname;
            appointmentFromDB.City = detailsVM.Appointment.City;
            appointmentFromDB.Country = detailsVM.Appointment.City;
            appointmentFromDB.Line1 = detailsVM.Appointment.Line1;
            appointmentFromDB.Zip = detailsVM.Appointment.Zip;

            if (User.IsInRole(SD.SuperAdminEndUser))
                appointmentFromDB.SalesPersonID = detailsVM.Appointment.SalesPersonID;

            await _db.SaveChangesAsync();
            TempData["SM"] = $"Appointment  has been edited successfully";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index(string searchKey, int productPage = 1)
        {
            // 1. Получаем объект типа ClaimsPrincipal
            var currentUser = this.User;

            // 2. Получаем объект идентификации юзера
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;

            // 3. Получаем доступ к объекту пользователя
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            StringBuilder param = new();
            param.Append("/Admin/Appointment?productPage=:");
            param.Append("&searchKey=");
            if (searchKey is not null)
                param.Append(searchKey);

            if (searchKey is null)
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

            var count = appointmentVM.Appointments.Count;
            appointmentVM.Appointments = appointmentVM.Appointments.OrderBy(x => x.AppointmentDay)
                                                                   .Skip((productPage - 1) * SD.PageSize)
                                                                   .Take(SD.PageSize)
                                                                   .ToList();

            appointmentVM.PaginationInfo = new()
            {
                CurrentPage = productPage,
                ItemPerPage = SD.PageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

            return View(appointmentVM);
        }
    }
}