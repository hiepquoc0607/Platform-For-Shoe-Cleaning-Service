namespace TP4SCS.Library.Models.Response.General
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; } = 200;
        public string? Token { get; set; }
        public string? Message { get; set; }
    }
}
