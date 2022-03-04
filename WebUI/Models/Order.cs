using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "Название")]
        public DateTime Posted { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string FoodName { get; set; }

    }
}
