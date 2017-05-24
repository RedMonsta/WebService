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
        //public byte[] Password { get; set; } = new byte[20];
        public int Status { get; set; } = 1;
    }


    public class ClientPlusOrder
    {
        public Client client { get; set; }
        public Order order { get; set; }
    }  
}