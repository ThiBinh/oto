using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanOto.Models;

namespace WebBanOto.ViewModels
{
    public class PTViewModels
    {   
        [Required(ErrorMessage = "You need to enter Full name")]
        public string Hoten { get; set; }
        [Required(ErrorMessage = "You need to enter Phone number")]
        public string Dienthoai { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Please select your preferred ")]
        public byte PHUONGTHUC { get; set; }
        public IEnumerable<PHUONGTHUC> Phuongthucs { get; set; }
    }
}