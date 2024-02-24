using System.ComponentModel.DataAnnotations;

namespace ChatSystem.Pages.Users.Validation
{
    public class BirthdayValidation : ValidationAttribute
    {
        private readonly int _minimumAge;

        public BirthdayValidation(int minimumAge)
        {
            _minimumAge = minimumAge;
        }
        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                var dateOfBirth = (DateTime)value;
                var age = DateTime.Today.Year - dateOfBirth.Year;

                if (age < _minimumAge)
                {
                    ErrorMessage = "You must be at least " + _minimumAge + " years old";
                    return false;
                }
            }
            return true;
        }
    }
}
