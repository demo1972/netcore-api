using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todolist.Models
{
    public class User
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }     
        [Required]
        public string Password { get; set; }
        private DateTime? create = null;
        public DateTime? Created
        {
            get { 
                return this.create != null ? this.create : DateTime.Now;
                
             }
            set { create = value; }
        }
            
    }
}