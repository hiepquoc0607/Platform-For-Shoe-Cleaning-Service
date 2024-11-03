using System.Text.RegularExpressions;
using TP4SCS.Library.Utils.StaticClass;

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
        public static string TranslateGeneralStatus(string? status)
        {
            if (string.IsNullOrEmpty(status)) return "Trạng Thái Null";

            var lowerStatus = status.ToLower();

            return lowerStatus switch
            {
                "unavailable" => "Ngưng Hoạt Động",
                "available" => "Hoạt Động",
                _ => "Trạng Thái Null"
            };
        }
        public static bool IsValidGeneralStatus(string status)
        {
            var validStatuses = new[] { StatusConstants.Available, StatusConstants.Unavailable };
            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }

        public static bool IsValidOrderStatus(string status)
        {
            var validStatuses = new[]
            {
                StatusConstants.CANCELED,
                StatusConstants.PENDING,
                StatusConstants.APPROVED,
                StatusConstants.PROCESSING,
                StatusConstants.STORAGE,
                StatusConstants.SHIPPING,
                StatusConstants.FINISHED,
                StatusConstants.ABANDONED
            };

            return validStatuses.Contains(status, StringComparer.OrdinalIgnoreCase);
        }


        public static bool IsEqual(string s1, string s2)
        {
            return string.Equals(s1.Trim(), s2.Trim(), StringComparison.OrdinalIgnoreCase);
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

        public string CheckDeleteEmployeesErrorType(string? old, List<int> input)
        {
            if (string.IsNullOrEmpty(old)) return "Empty";

            List<int> oldList = old.Split(",").Select(int.Parse).ToList();

            HashSet<int> oldSet = new HashSet<int>(oldList);

            foreach (int element in input)
            {
                if (!oldSet.Contains(element))
                {
                    return "Null";
                }
            }

            return "None";
        }

        public string CheckAddEmployeesErrorType(string? old, List<int> input)
        {
            List<int> oldList = new List<int>();

            if (!string.IsNullOrEmpty(old))
            {
                oldList = old.Split(',').Select(int.Parse).ToList();
            }

            if (oldList.Count > 5) return "Full";

            HashSet<int> oldSet = new HashSet<int>(oldList);

            foreach (int element in input)
            {
                if (oldSet.Contains(element))
                {
                    return "Existed";
                }
            }

            return "None";
        }

        public string DeleteEmployeesId(string? old, List<int> input)
        {
            List<int> oldList = new List<int>();

            if (!string.IsNullOrEmpty(old))
            {
                oldList = old.Split(',').Select(int.Parse).ToList();
            }

            foreach (int element in input)
            {
                oldList.Remove(element);
            }

            return string.Join(",", oldList);
        }

        public string AddEmployeeId(string? old, List<int> input)
        {
            List<int> oldList = new List<int>();

            if (!string.IsNullOrEmpty(old))
            {
                oldList = old.Split(',').Select(int.Parse).ToList();
            }

            foreach (int element in input)
            {
                oldList.Add(element);
            }

            return string.Join(",", oldList);
        }
    }
}
