namespace TP4SCS.Library.Models.Response.AssetUrl
{
    public class FileResponse
    {
        public string Url { get; set; } = string.Empty;

        public bool IsImage { get; set; }

        public string Type { get; set; } = string.Empty;
    }
}
