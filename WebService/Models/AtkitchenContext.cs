using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebService.Models
{
    public class AtkitchenContext : DbContext
    {
        public AtkitchenContext() : base("atkitchen_db")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}