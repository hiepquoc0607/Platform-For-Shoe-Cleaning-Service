using System.ComponentModel;

namespace TP4SCS.Library.Models.Request.Branch
{
    public class UpdateBranchStatisticRequest
    {
        public int PendingAmount { get; set; }

        public int ProcessingAmount { get; set; }

        public int FinishedAmount { get; set; }

        public int CanceledAmount { get; set; }
    }
}
