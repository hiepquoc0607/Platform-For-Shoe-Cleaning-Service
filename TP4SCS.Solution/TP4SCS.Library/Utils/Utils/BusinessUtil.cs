namespace TP4SCS.Library.Utils.Utils
{
    public class BusinessUtil
    {
        public bool CheckStatus(string status)
        {
            bool result = status.Trim().ToUpperInvariant() switch
            {
                "ACTIVE" => true,
                "INACTIVE" => true,
                _ => false
            };

            return result;
        }

        public bool CheckStatusForAdmin(string status)
        {
            bool result = status.Trim().ToUpperInvariant() switch
            {
                "ACTIVE" => true,
                "INACTIVE" => true,
                "SUSPENDED" => true,
                _ => false
            };

            return result;
        }

        public bool CheckDateTime(DateTime register, DateTime expired)
        {
            if (register >= expired) return false;

            return true;
        }
    }
}
