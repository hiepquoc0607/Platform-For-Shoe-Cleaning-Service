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

        public static string UpperCaseString(string input)
        {
            return input.ToUpper();
        }

        public bool CheckAccountStatusForAdmin(string status, StatusAdminRequest statusRequest)
        {
            if (status.Equals("ACTIVE", StringComparison.Ordinal) && statusRequest == StatusAdminRequest.ACTIVE)
            {
                return false;
            }

            if (status.Equals("INACTIVE", StringComparison.Ordinal) && statusRequest == StatusAdminRequest.INACTIVE)
            {
                return false;
            }

            if (status.Equals("SUSPENDED", StringComparison.Ordinal) && statusRequest == StatusAdminRequest.SUSPENDED)
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
        public static string TranslateGeneralStatus(string status)
        {
            string result = status switch
            {
                "INACTIVE" => "Ngưng Hoạt Động",
                _ => "Hoạt Động"
            };

            return result;
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
