using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class Dish
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Energy_value { get; set; }
        public string Price { get; set; }
    }
}