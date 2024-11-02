namespace TP4SCS.Library.Models.Request.AssetUrl
{
    public class AssetUrlRequest
    {
        public string Url { get; set; } = null!;
        public bool IsImage { get; set; }
        public string Type { get; set; } = null!;
    }
}
