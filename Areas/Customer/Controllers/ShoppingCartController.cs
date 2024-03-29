﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        //private int _pageSize = 3;

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new()
            {
                Products = new()
            };
        }

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }
        [HttpGet]
        public async Task<IActionResult> AppointmentConfirmation(int id)
        {
            ShoppingCartVM.Appointment = await _db.Appointments.FindAsync(id);
            List<ProductsForAppointment> listOfProducts = await _db.ProductsForAppointments.Where(x => x.AppointmentId == id)
                                                                                           .ToListAsync();
            foreach (var item in listOfProducts)
            {
                ShoppingCartVM.Products.Add(await _db.Products.Include(x => x.ProductTypes)
                                                              .Include(x => x.SpecialTags)
                                                              .FirstOrDefaultAsync(x => x.Id == item.ProductId));
            }

            ShoppingCartVM.PayPalConfig = Paypal.GetPayPalConfig();

            return View(ShoppingCartVM);
        }

        [HttpGet]
        public async Task<IActionResult> Index(int productPage = 1)
        {
            StringBuilder param = new();
            param.Append("/Customer/ShoppingCart?productPage=:");           
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);
           
            if ((listOfShoppingCart is not null) && (listOfShoppingCart.Count > 0))
            {
                foreach (var cartItem in listOfShoppingCart)
                {
                    Product product = await _db.Products.Include(x => x.ProductTypes)
                                                        .Include(x => x.SpecialTags)
                                                        .FirstOrDefaultAsync(x => x.Id == cartItem);

                    ShoppingCartVM.Products.Add(product);
                }
            }
            var count = ShoppingCartVM.Products.Count;
            ShoppingCartVM.Products = ShoppingCartVM.Products.OrderBy(x => x.Name)
                                                           .Skip((productPage - 1) * SD.PageSize)
                                                           .Take(SD.PageSize)
                                                           .ToList();

            ShoppingCartVM.PaginationInfo = new()
            {
                CurrentPage = productPage,
                ItemPerPage = SD.PageSize,
                TotalItems = count,
                UrlParam = param.ToString()
            };

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

            return RedirectToAction(nameof(AppointmentConfirmation), new { Id = appointmentId });
        }
        [HttpGet]
        public async Task<IActionResult> Success()
        {
            var result = PDTHolder.Success(Request.Query["tx"].ToString());
            int appointmentId = (int)TempData["AppointmentId"];
            result.AppointmentId = appointmentId;
            await _db.PayPalResponses.AddAsync(result);
            await _db.SaveChangesAsync();
            return View(result);
        }
    }
}