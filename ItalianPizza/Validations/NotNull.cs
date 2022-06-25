using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ItalianPizza.Validations
{
    public class NotNull : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                    ? new ValidationResult(false, "El campo es obligatorio")
                    : ValidationResult.ValidResult;
        }
    }
}