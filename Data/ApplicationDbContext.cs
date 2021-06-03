using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<ClientUser> ClientUsers { get; set; }

        public DbSet<IPBlackList> IPBlackLists { get; set; }

        public DbSet<MacBlackList> MacBlackLists { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductsForAppointment> ProductsForAppointments { get; set; }

        // Add new db context
        public DbSet<ProductTypes> ProductsTypes { get; set; }

        public DbSet<SpecialTag> SpecialTags { get; set; }
        public DbSet<PayPalResponse>PayPalResponses { get; set; }
    }
}