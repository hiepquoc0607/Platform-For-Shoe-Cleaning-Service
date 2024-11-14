using Mapster;
using MapsterMapper;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Ticket;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Models.Response.Ticket;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Library.Utils.Utils;
using TP4SCS.Repository.Interfaces;
using TP4SCS.Services.Interfaces;

namespace TP4SCS.Services.Implements
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ITicketCategoryRepository _ticketCategoryRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAssetUrlRepository _assetUrlRepository;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly Util _util;

        public TicketService(ITicketRepository ticketRepository,
            ITicketCategoryRepository ticketCategoryRepository,
            IAccountRepository accountRepository,
            IAssetUrlRepository assetUrlRepository,
            IEmailService emailService,
            IMapper mapper,
            Util util)
        {
            _ticketRepository = ticketRepository;
            _ticketCategoryRepository = ticketCategoryRepository;
            _accountRepository = accountRepository;
            _assetUrlRepository = assetUrlRepository;
            _emailService = emailService;
            _mapper = mapper;
            _util = util;
        }

        //Cancel Ticket
        public async Task<ApiResponse<TicketResponse>> CancelTicketAsync(int id)
        {
            var oldTicket = await _ticketRepository.GetUpdateTicketByIdAsync(id);

            if (oldTicket == null)
            {
                return new ApiResponse<TicketResponse>("error", 404, "Không Tìm Thấy Đơn Hỗ Trợ!");
            }

            if (oldTicket.Status.Equals(StatusConstants.PROCESSING))
            {
                return new ApiResponse<TicketResponse>("error", 400, "Đơn Hỗ Trợ Đã Vào Xử Lý!");
            }

            oldTicket.Status = StatusConstants.CANCELED;

            try
            {
                await _ticketRepository.UpdateAsync(oldTicket);

                return new ApiResponse<TicketResponse>("success", "Huỷ Đơn Hỗ Trợ Thành Công!", null, 200);
            }
            catch (Exception)
            {
                return new ApiResponse<TicketResponse>("error", 400, "Huỷ Đơn Hỗ Trợ Thất Bại!");
            }
        }

        //Create Child Ticket
        public async Task<ApiResponse<TicketResponse>> CreateChildTicketAsync(int userid, int id, CreateChildTicketRequest createChildTicketRequest)
        {
            var parentTicket = await _ticketRepository.GetUpdateTicketByIdAsync(id);

            if (parentTicket == null)
            {
                return new ApiResponse<TicketResponse>("error", 404, "Không Tìm Thấy Đơn Hỗ Trợ Chính!");
            }

            var newTicket = _mapper.Map<SupportTicket>(createChildTicketRequest);
            newTicket.ParentTicketId = id;
            newTicket.UserId = userid;
            newTicket.CategoryId = parentTicket.CategoryId;

            var newData = new TicketResponse();

            try
            {
                if (createChildTicketRequest.Assets != null)
                {
                    var newAsset = createChildTicketRequest.Assets.Adapt<List<AssetUrl>>();

                    await _ticketRepository.RunInTransactionAsync(async () =>
                    {
                        await _ticketRepository.CreateTicketAsync(newTicket);

                        newData = await _ticketRepository.GetTicketByIdAsync(newTicket.Id);

                        for (int i = 0; i < newAsset.Count; i++)
                        {
                            newAsset[i].TicketId = newTicket.Id;
                        }

                        await _assetUrlRepository.AddAssetUrlsAsync(newAsset);
                    });
                }
                else
                {
                    await _ticketRepository.CreateTicketAsync(newTicket);

                    newData = await _ticketRepository.GetTicketByIdAsync(newTicket.Id);
                }

                return new ApiResponse<TicketResponse>("success", "Tạo Đơn Hỗ Trợ Thành Công!", newData, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<TicketResponse>("error", 400, "Tạo Đơn Hỗ Trợ Thất Bại!");
            }

        }

        //Create Order Ticket
        public async Task<ApiResponse<TicketResponse>> CreateOrderTicketAsync(int id, CreateOrderTicketRequest createOrderTicketRequest)
        {
            try
            {
                var newTicket = _mapper.Map<SupportTicket>(createOrderTicketRequest);
                newTicket.UserId = id;
                newTicket.CategoryId = await _ticketCategoryRepository.GetOrderTicketCategoryIdAsync();

                var newData = new TicketResponse();

                var newAsset = createOrderTicketRequest.Assets.Adapt<List<AssetUrl>>();

                await _ticketRepository.RunInTransactionAsync(async () =>
                {
                    await _ticketRepository.CreateTicketAsync(newTicket);

                    newData = await _ticketRepository.GetTicketByIdAsync(newTicket.Id);

                    for (int i = 0; i < newAsset.Count; i++)
                    {
                        newAsset[i].TicketId = newTicket.Id;
                    }

                    await _assetUrlRepository.AddAssetUrlsAsync(newAsset);
                });

                return new ApiResponse<TicketResponse>("success", "Tạo Đơn Hỗ Trợ Thành Công!", newData, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<TicketResponse>("error", 400, "Tạo Đơn Hỗ Trợ Thất Bại!");
            }
        }

        //Create Ticket
        public async Task<ApiResponse<TicketResponse>> CreateTicketAsync(int id, CreateTicketRequest createTicketRequest)
        {
            var newTicket = _mapper.Map<SupportTicket>(createTicketRequest);
            newTicket.UserId = id;

            var newData = new TicketResponse();

            try
            {
                if (createTicketRequest.Assets != null)
                {
                    var newAsset = createTicketRequest.Assets.Adapt<List<AssetUrl>>();

                    await _ticketRepository.RunInTransactionAsync(async () =>
                    {
                        await _ticketRepository.CreateTicketAsync(newTicket);

                        newData = await _ticketRepository.GetTicketByIdAsync(newTicket.Id);

                        for (int i = 0; i < newAsset.Count; i++)
                        {
                            newAsset[i].TicketId = newTicket.Id;
                        }

                        await _assetUrlRepository.AddAssetUrlsAsync(newAsset);
                    });
                }
                else
                {
                    await _ticketRepository.CreateTicketAsync(newTicket);

                    newData = await _ticketRepository.GetTicketByIdAsync(newTicket.Id);
                }

                return new ApiResponse<TicketResponse>("success", "Tạo Đơn Hỗ Trợ Thành Công!", newData, 201);
            }
            catch (Exception)
            {
                return new ApiResponse<TicketResponse>("error", 400, "Tạo Đơn Hỗ Trợ Thất Bại!");
            }

        }

        //Get Ticket By Id
        public async Task<ApiResponse<TicketResponse?>> GetTicketByIdAsync(int id)
        {
            var ticket = await _ticketRepository.GetTicketByIdAsync(id);

            if (ticket == null)
            {
                return new ApiResponse<TicketResponse?>("error", 404, "Không Tìm Thấy Đơn Hỗ Trợ!");
            }

            return new ApiResponse<TicketResponse?>("success", "Lấy Thông Tin Hỗ Trợ Thành Công!", ticket, 200);
        }

        //Get Tickets
        public async Task<ApiResponse<IEnumerable<TicketsResponse>?>> GetTicketsAsync(GetTicketRequest getTicketRequest)
        {
            var (tickets, pagination) = await _ticketRepository.GetTicketsAsync(getTicketRequest);

            if (tickets == null)
            {
                return new ApiResponse<IEnumerable<TicketsResponse>?>("error", 404, "Không Tồn Tại Đơn Hỗ Trợ Nào!");
            }

            return new ApiResponse<IEnumerable<TicketsResponse>?>("success", "Lấy Thông Tin Hỗ Trợ Thành Công", tickets, 200, pagination);
        }

        //Get Tickets By Branch Id
        public async Task<ApiResponse<IEnumerable<TicketsResponse>?>> GetTicketsByBranchIdAsync(int id, GetBusinessTicketRequest getBusinessTicketRequest)
        {
            var (tickets, pagination) = await _ticketRepository.GetTicketsByBranchIdAsync(id, getBusinessTicketRequest);

            if (tickets == null)
            {
                return new ApiResponse<IEnumerable<TicketsResponse>?>("error", 404, "Không Tồn Tại Đơn Hỗ Trợ Nào!");
            }

            return new ApiResponse<IEnumerable<TicketsResponse>?>("success", "Lấy Thông Tin Hỗ Trợ Thành Công", tickets, 200, pagination);
        }

        //Get Tickets By Business Id
        public async Task<ApiResponse<IEnumerable<TicketsResponse>?>> GetTicketsByBusinessAsync(int id, GetBusinessTicketRequest getBusinessTicketRequest)
        {
            var (tickets, pagination) = await _ticketRepository.GetTicketsByBusinessIdAsync(id, getBusinessTicketRequest);

            if (tickets == null)
            {
                return new ApiResponse<IEnumerable<TicketsResponse>?>("error", 404, "Không Tồn Tại Đơn Hỗ Trợ Nào!");
            }

            return new ApiResponse<IEnumerable<TicketsResponse>?>("success", "Lấy Thông Tin Hỗ Trợ Thành Công", tickets, 200, pagination);
        }

        //Update Ticket Status
        public async Task<ApiResponse<TicketResponse>> UpdateTicketStatusAsync(int moderatorId, int id, UpdateTicketStatusRequest updateTicketStatusRequest)
        {
            var oldTicket = await _ticketRepository.GetUpdateTicketByIdAsync(id);

            if (oldTicket == null)
            {
                return new ApiResponse<TicketResponse>("error", 404, "Không Tìm Thấy Đơn Hỗ Trợ!");
            }

            if (!_util.CheckTicketStatus(updateTicketStatusRequest.Status))
            {
                return new ApiResponse<TicketResponse>("error", 400, "Trạng Thái Không Hợp Lệ!");
            }

            var oldStatus = oldTicket.Status;

            oldTicket.Status = updateTicketStatusRequest.Status.ToUpperInvariant();
            oldTicket.ModeratorId = moderatorId;

            try
            {
                await _ticketRepository.UpdateTicketAsync(oldTicket);

                string userEmail = await _accountRepository.GetAccountEmailByIdAsync(oldTicket.UserId);
                string emailSubject = "ShoeCareHub Đơn Khiếu Nại";
                string emailBody;

                if (oldTicket.Status.Equals(StatusConstants.OPENING))
                {
                    emailBody = "Đơn Khiếu Nại Của Bạn Đã Được Tiếp Nhận Và Đang Được Xử Lý!";
                }
                else if (oldStatus.Equals(StatusConstants.OPENING) && oldTicket.Status.Equals(StatusConstants.CLOSED))
                {
                    emailBody = "Đơn Khiếu Nại Của Bạn Đã Bị Từ Chối Vui Lòng Kiểm Tra Lại Thông Tin Và Thử Lại!";
                }
                else
                {
                    emailBody = "Đơn Khiếu Nại Của Bạn Đã Hoàn Tất Việc Xử Lý!";
                }

                _ = _emailService.SendEmailAsync(userEmail, emailSubject, emailBody);

                return new ApiResponse<TicketResponse>("error", "Cập Nhập Trạng Thái Đơn Hỗ Trợ Thành Công!", null, 200);
            }
            catch (Exception)
            {
                return new ApiResponse<TicketResponse>("error", 400, "Cập Nhập Trạng Thái Đơn Hỗ Trợ Thất Bại!");
            }
        }
    }
}
