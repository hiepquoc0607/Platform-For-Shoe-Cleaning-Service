using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.Feedback;
using TP4SCS.Library.Models.Response.Material;
using TP4SCS.Library.Models.Response.Service;

namespace TP4SCS.Library.Models.Response.OrderDetail
{
    public class OrderDetailResponseV2
    {
        public int Id { get; set; }
        public BranchResponse Branch { get; set; } = null!;

        public ServiceResponse? Service { get; set; }

        public MaterialResponse? Material { get; set; }

        public virtual FeedbackResponse? Feedback { get; set; }

        public decimal Price { get; set; }
    }
}
