namespace TP4SCS.Library.Utils
{
    public class Util
    {
        public Util()
        {
        }

        public bool IsValidPassword(string password)
        {
            /*
            (?=.*[0-9]) contain at least 1 number digit character
            (?=.*[a-z]) contain at least 1 lower case character
            (?=.*[A-Z]) contain at least 1 upper case character
            (?=.*[\W_]) contain at least 1 special character
            {8,} at least 8 character
             */
            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*\W)(?!.* ).{8,}$");

            return regex.IsMatch(password);
        }
    }
}
