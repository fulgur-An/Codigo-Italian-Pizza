using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class Time : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (text == null)
            {
                return new ValidationResult(false, "El campo es obligatorio");
            }

            string regularPhrase = @"^([01]?[0-9]|2[0-3])$";

            if (!Regex.IsMatch(text, regularPhrase))
            {
                return new ValidationResult(false, "El campo debe contener una hora en formato de 24 hrs");
            }

            return ValidationResult.ValidResult;
        }
    }
}