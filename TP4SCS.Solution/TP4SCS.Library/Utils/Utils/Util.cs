using System.Text.RegularExpressions;

namespace TP4SCS.Library.Utils.Utils
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

        public static string UpperCaseStringStatic(string input)
        {
            return input.ToUpper();
        }

        public bool CheckStatusForAdmin(string status, string statusRequest)
        {
            return !string.Equals(status, statusRequest, StringComparison.OrdinalIgnoreCase);
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

            var lowerStatus = status.ToLower();

            return lowerStatus switch
            {
                "inactive" => "Ngưng Hoạt Động",
                _ => "Hoạt Động"
            };
        }

        public static string? TranslatePromotionStatus(string? status)
        {
            if (string.IsNullOrEmpty(status)) return null;

            var lowerStatus = status.ToLower();

            return lowerStatus switch
            {
                "expired" => "Hết hạn",
                "available" => "Còn hiệu lực",
                _ => "Trạng thái Null"
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

        public bool CheckAccountRole(string role)
        {
            bool result = role.ToUpper() switch
            {
                "OWNER" => true,
                "EMPLOYEE" => true,
                "ADMIN" => true,
                "MODERATOR" => true,
                "CUSTOMER" => true,
                _ => false
            };

            return result;
        }

        public bool CheckBranchStatus(string status)
        {
            bool result = status.ToUpper() switch
            {
                "ACTIVE" => true,
                "INACTIVE" => true,
                "SUSPENDED" => true,
                _ => false
            };

            return result;
        }

        public string CheckDeleteEmployeesErrorType(string? old, string input)
        {
            if (string.IsNullOrEmpty(old)) return "Empty";
            if (!input.Trim().Trim().Split(", ").All(item => old.Trim().Split(", ").Contains(item)))
            {
                return "Exist";
            }
            if (!Regex.IsMatch(input, @"^[0-9, ]*$")) return "Invalid";

            return "None";
        }

        public string CheckAddEmployeesErrorType(string? old, string input)
        {
            if (input.Split(',').Length > 5) return "Full";
            if (!string.IsNullOrEmpty(old) && input.Trim().Trim().Split(", ").All(item => old.Trim().Split(", ").Contains(item)))
            {
                return "Exist";
            }
            if (!Regex.IsMatch(input, @"^[0-9, ]*$")) return "Invalid";

            return "None";
        }

        public string DeleteEmployeesId(string? old, string input)
        {
            var oldArray = old?.Split(",").Select(item => item.Trim()).ToArray();
            var inputArray = input.Split(",").Select(item => item.Trim()).ToArray();

            var resultArray = oldArray?.Except(inputArray) ?? Array.Empty<string>();

            return string.Join(", ", resultArray);
        }

        public string AddEmployeeId(string? old, string input)
        {
            var oldString = old?.Split(',').Select(x => x.Trim()) ?? Array.Empty<string>();
            var inputString = input.Split(',').Select(x => x.Trim());

            return string.Join(", ", oldString.Concat(inputString));
        }
    }
}
