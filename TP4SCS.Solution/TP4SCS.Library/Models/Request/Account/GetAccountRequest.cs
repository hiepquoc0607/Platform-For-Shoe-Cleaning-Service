namespace TP4SCS.Library.Models.Request.General
{
    public enum SortOption
    {
        Id = 0,
        Email = 1,
        Fullname = 2,
    }

    public class GetAccountRequest
    {
        public string? SearchKey { get; set; }
        public SortOption SortBy { get; set; } = SortOption.Id;
        public bool IsDecsending { get; set; } = false;
        public int PageSize { get; set; } = 10;
        public int PageNum { get; set; } = 1;
    }
}
