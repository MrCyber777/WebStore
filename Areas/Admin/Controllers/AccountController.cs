
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;
using WebStore.Models.ViewModel;
using WebStore.Utility;


namespace WebStore.Areas.Customer
{
    
    [Area(nameof(Customer))]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public RegisterViewModel RegisterInput { get; set; }
        public string ReturnUrl { get; set; }     
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                string userIP = GetUserIPOrNull();
                string userMAC = GetUserMACOrNull();
                var res = await IPIsLocked(userIP);              
                if(res)
                {
                    ModelState.AddModelError("", "Your IP is banned!. Good Bye");
                    return View(model);
                }
                if (await MacIsLocked(userMAC))
                {
                    ModelState.AddModelError("", "Your MAC address is banned!");
                    return View(model);
                }
                if(string.IsNullOrWhiteSpace(userIP)||string.IsNullOrWhiteSpace(userMAC))
                {
                    ModelState.AddModelError("", "IP or MAC does not exist");
                    return View(model);
                }
                ClientUser clientUser = new ClientUser { UserName = model.Name, Email = model.Email,Address=model.Address,IP=userIP,UserMac=userMAC };
                var result = await _userManager.CreateAsync(clientUser, model.Password);

                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync(SD.User))
                        await _roleManager.CreateAsync(new IdentityRole(SD.User));
                    await _userManager.AddToRoleAsync(clientUser, SD.User);

                    await _signInManager.SignInAsync(clientUser, false);

                    return RedirectToAction("Index", "Home");
                }
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }    
        private string GetUserIPOrNull()
        {
            IPHostEntry hostIpInfo = Dns.GetHostEntry(Dns.GetHostName());
            string currentIp = Convert.ToString(hostIpInfo.AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            return (!string.IsNullOrWhiteSpace(currentIp)) ? currentIp : null;
        }
        private string GetUserMACOrNull()
        {
            ManagementClass mc = new("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            string macAddress = string.Empty;

            foreach(var item in moc)
            {
                if (string.IsNullOrEmpty(macAddress))
                {
                    if ((bool)item["IPEnabled"])
                    {
                        macAddress = item["MacAddress"].ToString();
                        item.Dispose();
                        break;
                    }
                }
                item.Dispose();
            }

            return (!string.IsNullOrEmpty(macAddress)) ? macAddress : null;
        }
       private async Task<bool> IPIsLocked(string userIP)
        {
            var blockedIP = await _db.IPBlackLists.FirstOrDefaultAsync(x => x.Address == userIP);
            return (blockedIP is null) ? false : true;
        }
       private async Task<bool>MacIsLocked(string userMac)
        {
            var blockedMac = await _db.MacBlackLists.FirstOrDefaultAsync(x => x.Address == userMac);
            return (blockedMac is null) ? false : true;
        }
    }
}
