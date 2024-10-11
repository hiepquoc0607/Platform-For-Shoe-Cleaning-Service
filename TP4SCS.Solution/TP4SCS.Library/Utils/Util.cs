namespace TP4SCS.Library.Utils
{
    public class Util
    {
        public Util()
        {
        }

        public string CheckPasswordErrorType(string password)
        {
            if (password.Length < 8)
            {
                return "Length";
            }
            if (!password.Any(char.IsDigit))
            {
                return "Number";
            }
            if (!password.Any(char.IsLower))
            {
                return "Lower";
            }
            if (!password.Any(char.IsUpper))
            {
                return "Upper";
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return "Special";
            }

            return "None";

        }

        public string UpperCaseString(string input)
        {
            return input.ToUpper();
        }
    }
}
