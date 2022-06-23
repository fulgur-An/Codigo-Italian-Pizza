using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class AlphabeticCharacter : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = value as string;

            if(text == null)
            {
                return new ValidationResult(false, "El campo es obligatorio");
            }

            string regularPhrase = @"^[a-zA-ZáéíóúüñÁÉÍÓÚÑ]+(?:[\s][a-zA-ZáéíóúüñÁÉÍÓÚÑ]+)*$";

            if (!Regex.IsMatch(text, regularPhrase))
            {
                return new ValidationResult(false, "El campo solo debe contener caracteres alfabéticos");
            }
            return ValidationResult.ValidResult;
        }
    }
}