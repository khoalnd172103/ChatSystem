using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
	public class UserNameValidation : ValidationAttribute
	{
		public override bool IsValid(object? value)
		{
			if (value == null)
			{
				return false;
			}
			else
			{
				string userName = value.ToString();
				if (userName.Contains(' '))
				{
					ErrorMessage = "Username cannot contain whitespace.";
					return false;
				}
			}
			return true;
		}
	}
}