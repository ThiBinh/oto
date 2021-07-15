using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebBanOto.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            
            if (value == null) { ErrorMessage = "Invalid date"; }
            else
            {

                DateTime dateTime;
                dateTime = DateTime.Parse(value.ToString());
                return (dateTime > DateTime.Now);

            }
            return (false);
           
        }

    }
}