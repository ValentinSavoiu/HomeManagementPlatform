using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mss_project.CustomValidations
{
    public class Name : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is null)
                return true;
            // checking that name only contains letters, spaces and hyphens
            string name = value.ToString();
            bool isBad = name.Any(x => !char.IsLetter(x) && x != ' ' && x != '-');
            return !isBad;
        }
    }
}