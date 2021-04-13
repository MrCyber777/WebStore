using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModel;

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
            appointmentVM.Appointments = await _db.Appointments.ToListAsync();
            return View(appointmentVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            Appointment appointmentFromDB = await _db.Appointments.FindAsync(id);
            if (appointmentFromDB == null)
                return NotFound();

            return View(appointmentFromDB);
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
