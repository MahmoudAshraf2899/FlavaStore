using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Store.Models
{
    public class Bike
    {
        public int Id { get; set; }
        public Make Make { get; set; }
       
        [RegularExpression("^[1-9]*$",ErrorMessage = "The Make dropdownlist is required.")]
        [Required]
        public  int MakeID { get; set; }  
        public Model Model { get; set; }

        [RegularExpression("^[1-9]*$", ErrorMessage = "The Model dropdownlist is required.")]

        [Required]
        public int ModelID { get; set; }      
        [Required]
        public int Year { get; set; }
        
        [Required]
        public int Mileage { get; set; }
        
        [Required]
        public string Features { get; set; }
        
        [Required]
        public string SellerName { get; set; }
        
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        public string SellerEmail { get; set; }
        
        [Required]
        public int SellerPhone { get; set; }
       
        [Required]
        public int Price { get; set; }
        
        [Required]
        public string Currency { get; set; }
        public string ImagePath { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
