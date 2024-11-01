using System.Text;

namespace TP4SCS.Library.Utils.Utils
{
    public class StringUtil
    {
        public string CapitalizeFormat(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            StringBuilder result = new StringBuilder(input.Length);
            bool capitalize = true;

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    result.Append(c);
                    capitalize = true;
                }
                else if (capitalize)
                {
                    result.Append(char.ToUpper(c));
                    capitalize = false;
                }
                else
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }
    }
}
