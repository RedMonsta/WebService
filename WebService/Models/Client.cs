using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class Client
    {
        //public int Id { get; set; }
        [Key]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}