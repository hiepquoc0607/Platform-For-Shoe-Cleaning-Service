using System.Text.Json.Serialization;

namespace TP4SCS.Library.Models.Response.General
{
    public class Pagination
    {
        public Pagination(int totalItems, int pageSize, int currentPage, int totalPages)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPages = totalPages;
        }

        public int TotalItems { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }

    public class Result<T>
    {
        public Result(string status, string message, T? data, Pagination? pagination = null)
        {
            Status = status;
            Message = message;
            Data = data;
            Pagination = pagination;
        }

        public Result(string status, int statusCode, string message)
        {
            Status = status;
            StatusCode = statusCode;
            Message = message;
        }

        public string Status { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int StatusCode { get; set; } = 200;

        public string Message { get; set; } = string.Empty;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Pagination? Pagination { get; set; }
    }
}
