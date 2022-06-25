using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class Phone : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (text == null)
            {
                return new ValidationResult(false, "El campo es obligatorio");
            }

            string regularPhrase = @"^[0-9]{10}$";

            if (!Regex.IsMatch(text, regularPhrase))
            {
                return new ValidationResult(false, "El campo debe contener un número de contacto (10 dígitos)");
            }

            return ValidationResult.ValidResult;
        }
    }
}