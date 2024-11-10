using TP4SCS.Library.Models.Request.Ticket;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Ticket;

namespace TP4SCS.Services.Interfaces
{
    public interface ITicketService
    {
        Task<ApiResponse<IEnumerable<TicketsResponse>?>> GetTicketsAsync(GetTicketRequest getTicketRequest);

        Task<ApiResponse<TicketResponse?>> GetTicketByIdAsync(int id);

        Task<ApiResponse<TicketResponse>> CreateTicketAsync(CreateTicketRequest createTicketRequest);

        Task<ApiResponse<TicketResponse>> CreateOrderTicketAsync(CreateOrderTicketRequest createOrderTicketRequest);

        Task<ApiResponse<TicketResponse>> CreateChildTicketAsync(CreateChildTicketRequest createChildTicketRequest);

        Task<ApiResponse<TicketResponse>> UpdateTicketStatusAsync(int id, UpdateTicketStatusRequest updateTicketStatusRequest);

        Task<ApiResponse<TicketResponse>> CancelTicketAsync(int id);
    }
}
