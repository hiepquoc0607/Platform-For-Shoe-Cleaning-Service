using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Request.Transaction;
using TP4SCS.Library.Services;
using TP4SCS.Library.Models.Response;
using TP4SCS.Library.Models.Response.Transaction;
using System.Linq;
using TP4SCS.Library.Models.Response.General;

namespace TP4SCS.WebAPI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(IMapper mapper, ITransactionService transactionService)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // GET: api/transactions
        [HttpGet]
        public async Task<IActionResult> GetTransactionsAsync([FromQuery] PagedRequest pagedRequest)
        {
            try
            {
                // Gọi service để lấy các giao dịch với phân trang
                var pagedResponse = await _transactionService.GetTransactionsAsync(
                    pagedRequest.PageIndex,
                    pagedRequest.PageSize,
                    pagedRequest.OrderBy
                );

                // Trả về ResponseObject với PagedResponse
                return Ok(new ResponseObject<PagedResponse<TransactionResponse>>(
                    "Lấy giao dịch thành công",
                    new PagedResponse<TransactionResponse>(
                        pagedResponse.Items.Select(t => _mapper.Map<TransactionResponse>(t)),
                        pagedResponse.TotalCount,
                        pagedRequest.PageIndex,
                        pagedRequest.PageSize
                    )
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/transactions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionByIdAsync(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                {
                    return NotFound($"Transaction with ID {id} not found.");
                }

                var response = _mapper.Map<TransactionResponse>(transaction);
                return Ok(new ResponseObject<TransactionResponse>("Lấy giao dịch thành công", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/transactions/account/{accountId}
        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetTransactionsByAccountIdAsync(int accountId, [FromQuery] PagedRequest pagedRequest)
        {
            try
            {
                // Lấy các giao dịch theo AccountId với phân trang
                var pagedResponse = await _transactionService.GetTransactionsByAccountIdAsync(
                    accountId,
                    pagedRequest.PageIndex,
                    pagedRequest.PageSize
                );

                if (!pagedResponse.Items.Any())
                {
                    return NotFound($"No transactions found for account with ID {accountId}.");
                }

                // Trả về ResponseObject với PagedResponse
                return Ok(new ResponseObject<PagedResponse<TransactionResponse>>(
                    "Lấy giao dịch thành công",
                    new PagedResponse<TransactionResponse>(
                        pagedResponse.Items.Select(t => _mapper.Map<TransactionResponse>(t)),
                        pagedResponse.TotalCount,
                        pagedRequest.PageIndex,
                        pagedRequest.PageSize
                    )
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // POST: api/transactions
        [HttpPost]
        public async Task<IActionResult> CreateTransactionAsync([FromBody] TransactionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Transaction object is null.");
                }

                var transaction = _mapper.Map<Transaction>(request);
                await _transactionService.CreateTransactionAsync(transaction);

                var response = _mapper.Map<TransactionResponse>(transaction);
                return CreatedAtAction(nameof(GetTransactionByIdAsync), new { id = transaction.Id },
                                       new ResponseObject<TransactionResponse>("Tạo giao dịch thành công", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/transactions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionAsync(int id, [FromBody] TransactionRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest("Transaction object is null.");
                }

                var transaction = _mapper.Map<Transaction>(request);
                await _transactionService.UpdateTransactionAsync(id, transaction);

                var response = _mapper.Map<TransactionResponse>(transaction);
                return Ok(new ResponseObject<TransactionResponse>("Cập nhật giao dịch thành công", response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/transactions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionAsync(int id)
        {
            try
            {
                var existingTransaction = await _transactionService.GetTransactionByIdAsync(id);
                if (existingTransaction == null)
                {
                    return NotFound($"Transaction with ID {id} not found.");
                }

                await _transactionService.DeleteTransactionAsync(id);
                return NoContent(); // 204 No Content, indicates successful deletion
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
