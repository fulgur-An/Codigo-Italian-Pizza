using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class DecimalNumber : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if (text == null)
            {
                return new ValidationResult(false, "El campo es obligatorio");
            }

            string regularPhrase = @"^[0-9]+([.,][0-9]+)?$";

            if (!Regex.IsMatch(text, regularPhrase))
            {
                return new ValidationResult(false, "El campo solo debe contener números enteros o decimales");
            }

            return ValidationResult.ValidResult;
        }
    }
}
