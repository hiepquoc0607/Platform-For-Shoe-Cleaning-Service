using Microsoft.IdentityModel.Tokens;
using TP4SCS.Library.Models.Request.Account;

namespace TP4SCS.Library.Utils
{
    public class Util
    {
        public Util()
        {
        }

        public string CheckPasswordErrorType(string password)
        {
            bool hasDigit = false;
            bool hasLower = false;
            bool hasUpper = false;
            bool hasSpecial = false;

            if (password.Length < 8)
            {
                return "Length";
            }

            foreach (char ch in password)
            {
                if (char.IsDigit(ch)) hasDigit = true;
                else if (char.IsLower(ch)) hasLower = true;
                else if (char.IsUpper(ch)) hasUpper = true;
                else if (!char.IsLetterOrDigit(ch)) hasSpecial = true;

                if (hasDigit && hasLower && hasUpper && hasSpecial)
                {
                    return "None";
                }
            }

            if (!hasDigit) return "Number";
            if (!hasLower) return "Lower";
            if (!hasUpper) return "Upper";
            if (!hasSpecial) return "Special";

            return "None";
        }

        public string UpperCaseString(string input)
        {
            return input.ToUpper();
        }
        public static string UpperCaseStringStatic(string input)
        {
            return input.ToUpper();
        }

        public string LowerCaseString(string input)
        {
            return input.ToLower();
        }

        public bool CheckAccountStatusForAdmin(string status, StatusAdminRequest statusRequest)
        {
            Dictionary<string, StatusAdminRequest> StatusMap = new()
            {
                { "ACTIVE", StatusAdminRequest.ACTIVE },
                { "INACTIVE", StatusAdminRequest.INACTIVE },
                { "SUSPENDED", StatusAdminRequest.SUSPENDED }
            };

            if (StatusMap.TryGetValue(status, out StatusAdminRequest mappedStatus) && mappedStatus == statusRequest)
            {
                return false;
            }

            return true;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool CompareHashedPassword(string password1, string password2)
        {
            return BCrypt.Net.BCrypt.Verify(password1, password2);
        }

        public string TranslateAccountStatus(string status)
        {
            string result = status switch
            {
                "INACTIVE" => "Ngưng Hoạt Động",
                "SUSPENDED" => "Đã Khoá",
                _ => "Hoạt Động"
            };

            return result;
        }
        public static string? TranslateGeneralStatus(string? status)
        {
            if (string.IsNullOrEmpty(status)) return null;

            return status switch
            {
                "INACTIVE" => "Ngưng Hoạt Động",
                _ => "Hoạt Động"
            };
        }
        public string TranslateAccountRole(string role)
        {
            string result = role switch
            {
                "OWNER" => "Chủ Cung Cấp",
                "EMPLOYEE" => "Nhân Viên",
                "ADMIN" => "Quản Trị Viên",
                "MODERATOR" => "Người Điều Hành",
                _ => "Khách Hàng"
            };

            return result;
        }
    }
}
