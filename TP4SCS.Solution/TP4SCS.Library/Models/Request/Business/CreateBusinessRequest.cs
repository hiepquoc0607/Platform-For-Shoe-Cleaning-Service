namespace TP4SCS.Library.Models.Request.BusinessProfile
{
    public class CreateBusinessRequest
    {

        public int OwnerId { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
