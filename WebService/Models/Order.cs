using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //public string Surname { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }    
        public string Dish { get; set; }
        public string Status { get; set; }
        public string Nickname { get; set; }

    }
}