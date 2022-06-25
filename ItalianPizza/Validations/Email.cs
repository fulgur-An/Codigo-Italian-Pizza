
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class Email : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (text == null)
            {
                return new ValidationResult(false, "El campo es obligatorio");
            }

            string regularPhrase = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

            if (!Regex.IsMatch(text, regularPhrase))
            {
                return new ValidationResult(false, "El campo debe contener un correo eléctronico");
            }

            return ValidationResult.ValidResult;
        }
    }
}