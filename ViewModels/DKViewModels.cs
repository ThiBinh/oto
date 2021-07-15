using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanOto.Models;

namespace WebBanOto.ViewModels
{
    public class DKViewModels
    {
        [Required(ErrorMessage = "Bạn cần nhập họ tên")]
        public string Hoten { get; set; }
        
        public string Dienthoai { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string Email { get; set; }


        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",ApplyFormatInEditMode = true)]
        
        //[Required(ErrorMessage = "Ngày không được bỏ trống")]
        [FutureDate(ErrorMessage = "Invalid date")]
        public string Ngay { get; set;}
        [Required(ErrorMessage = "Please select your preferred time ")]
        public byte GIO { get; set; }
        public IEnumerable<GIO> Gios { get; set; }
    }
}