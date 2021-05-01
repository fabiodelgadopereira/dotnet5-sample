using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Kanban.Models
{
    public class User 
    {
        [Key]
         public  int id { get; internal set; }
        public string login { get; set; }
         public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
    }
}