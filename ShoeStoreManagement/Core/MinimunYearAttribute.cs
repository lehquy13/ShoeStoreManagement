using System.ComponentModel.DataAnnotations;

namespace ShoeStoreManagement.Core
{
    public class MinimunYearAttribute : ValidationAttribute
    {
        int _minYear;
        DateTime _maxDate;
        //string errorMessage = "";

        public MinimunYearAttribute(int minYear)
        {
            _minYear = minYear;
            _maxDate = DateTime.Now;
            ErrorMessage = "Birthday is not valid!";
        }

        public override bool IsValid(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                if (date >= _maxDate)
                {
                    ErrorMessage = "Birthday is not valid!";
                    return false;
                }

                ErrorMessage = "Birthday is not valid! Year is too soon?";
                return date.Year >= _minYear;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString);
        }
    }
}
