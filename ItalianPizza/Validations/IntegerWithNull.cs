using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class IntegerWithNull : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            string regularPhrase = @"(\d+)+$";
            try
            {
                if (!Regex.IsMatch(text, regularPhrase))
                {
                    return new ValidationResult(false, "El campo solo debe contener números enteros");
                }
            }
            catch (ArgumentNullException)
            {

            }


            return ValidationResult.ValidResult;
        }
    }
}