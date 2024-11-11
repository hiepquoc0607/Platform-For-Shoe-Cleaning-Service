using Microsoft.EntityFrameworkCore;
using System.Text;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.Account;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Library.Models.Response.Branch;
using TP4SCS.Library.Models.Response.General;
using TP4SCS.Library.Utils.StaticClass;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(Tp4scsDevDatabaseContext dbConext) : base(dbConext)
        {
        }

        public async Task CreateAccountAsync(Account account)
        {
            await InsertAsync(account);
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Accounts.SingleOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(IEnumerable<Account>?, Pagination)> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            var accounts = _dbContext.Accounts.AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(getAccountRequest.SearchKey))
            {
                string searchKey = getAccountRequest.SearchKey;
                accounts = accounts.Where(a =>
                    EF.Functions.Like(a.FullName, $"%{searchKey}%") ||
                    EF.Functions.Like(a.Email, $"%{searchKey}%"));
            }

            //Status Sort
            if (getAccountRequest.Status != null)
            {
                accounts = getAccountRequest.Status switch
                {
                    AccountStatus.ACTIVE => accounts.Where(a => a.Status.Equals(StatusConstants.ACTIVE)),
                    AccountStatus.INACTIVE => accounts.Where(a => a.Status.Equals(StatusConstants.INACTIVE)),
                    AccountStatus.SUSPENDED => accounts.Where(a => a.Status.Equals(StatusConstants.SUSPENDED)),
                    _ => accounts
                };
            }

            //Order Sort
            if (getAccountRequest.SortBy != null)
            {
                accounts = getAccountRequest.SortBy switch
                {
                    AccountSearchOption.EMAIL => getAccountRequest.IsDecsending
                                ? accounts.OrderByDescending(a => a.Email)
                                : accounts.OrderBy(a => a.Email),
                    AccountSearchOption.FULLNAME => getAccountRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.FullName)
                                  : accounts.OrderBy(a => a.FullName),
                    AccountSearchOption.STATUS => getAccountRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.Status)
                                  : accounts.OrderBy(a => a.Status),
                    _ => accounts
                };
            }

            //Count Total Data
            int totalData = await accounts.AsNoTracking().CountAsync();

            //Paging
            int skipNum = (getAccountRequest.PageNum - 1) * getAccountRequest.PageSize;
            accounts = accounts.Skip(skipNum).Take(getAccountRequest.PageSize);

            var result = await accounts.Select(a => new Account
            {
                Id = a.Id,
                Email = a.Email,
                FullName = a.FullName,
                Phone = a.Phone,
                Gender = a.Gender,
                Dob = a.Dob,
                ImageUrl = a.ImageUrl,
                CreatedByOwnerId = a.CreatedByOwnerId,
                Role = a.Role,
                Status = a.Status,
            })
                .ToListAsync();

            int totalPage = (int)Math.Ceiling((decimal)totalData / getAccountRequest.PageSize);

            var pagination = new Pagination(totalData, getAccountRequest.PageSize, getAccountRequest.PageNum, totalPage);

            return (result, pagination);
        }

        public async Task<Account?> GetAccountLoginByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Accounts.AsNoTracking().FirstOrDefaultAsync(a => a.Email.Equals(email));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> IsEmailExistedAsync(string email)
        {
            return await _dbContext.Accounts.AsNoTracking().AnyAsync(a => a.Email.Equals(email));
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await UpdateAsync(account);
        }

        public async Task<bool> IsPhoneExistedAsync(string phone)
        {
            return await _dbContext.Accounts.AsNoTracking().AnyAsync(a => a.Phone.Equals(phone));
        }

        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _dbContext.Accounts.AsNoTracking().MaxAsync(a => a.Id);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Email.Equals(email));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> CountAccountDataAsync()
        {
            return await _dbContext.Accounts.AsNoTracking().CountAsync();
        }

        public async Task<Account?> GetAccountByIdForAdminAsync(int id)
        {
            try
            {
                return await _dbContext.Accounts.SingleOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<(IEnumerable<EmployeeResponse>?, Pagination)> GetEmployeesAsync(GetEmployeeRequest getEmployeeRequest)
        {
            var branchEmployee = await _dbContext.BusinessBranches
                .AsNoTracking()
                .Where(b => b.BusinessId == getEmployeeRequest.BusinessId)
                .Select(b => b.EmployeeIds)
                .ToListAsync();

            var ids = new StringBuilder();

            foreach (var item in branchEmployee)
            {
                if (ids.Length > 0)
                {
                    ids.Append(",");
                }
                ids.Append(item);
            }

            var idList = ids.Length == 0 ? new List<int>() : ids.ToString()
                .Split(',')
                .Where(id => int.TryParse(id, out _))
                .Select(int.Parse)
                .ToList();


            var accounts = _dbContext.Accounts
                .Where(a => idList.Contains(a.Id))
                .Select(a => new EmployeeResponse
                {
                    Id = a.Id,
                    Email = a.Email,
                    FullName = a.FullName,
                    Phone = a.Phone,
                    Gender = a.Gender,
                    Dob = a.Dob,
                    ImageUrl = a.ImageUrl,
                    CreatedByOwnerId = a.CreatedByOwnerId,
                    Status = a.Status,
                    Branch = _dbContext.BusinessBranches
                        .AsNoTracking()
                        .Where(bb => bb.EmployeeIds != null && bb.EmployeeIds.Contains(a.Id.ToString()))
                        .Select(bb => new EmployeeBranchResponse
                        {
                            Id = bb.Id,
                            BusinessId = bb.BusinessId,
                            Name = bb.Name,
                            Address = bb.Address,
                            Status = bb.Status
                        })
                        .FirstOrDefault()!
                })
                .AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(getEmployeeRequest.SearchKey))
            {
                string searchKey = getEmployeeRequest.SearchKey;
                accounts = accounts.Where(a =>
                    EF.Functions.Like(a.FullName, $"%{searchKey}%") ||
                    EF.Functions.Like(a.Email, $"%{searchKey}%"));
            }

            //Status Sort
            if (getEmployeeRequest.Status != null)
            {
                accounts = getEmployeeRequest.Status switch
                {
                    AccountStatus.ACTIVE => accounts.Where(a => a.Status.Equals(StatusConstants.ACTIVE)),
                    AccountStatus.INACTIVE => accounts.Where(a => a.Status.Equals(StatusConstants.INACTIVE)),
                    AccountStatus.SUSPENDED => accounts.Where(a => a.Status.Equals(StatusConstants.SUSPENDED)),
                    _ => accounts
                };
            }

            //Order Sort
            if (getEmployeeRequest.SortBy != null)
            {
                accounts = getEmployeeRequest.SortBy switch
                {
                    AccountSearchOption.EMAIL => getEmployeeRequest.IsDecsending
                                ? accounts.OrderByDescending(a => a.Email)
                                : accounts.OrderBy(a => a.Email),
                    AccountSearchOption.FULLNAME => getEmployeeRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.FullName)
                                  : accounts.OrderBy(a => a.FullName),
                    AccountSearchOption.STATUS => getEmployeeRequest.IsDecsending
                                  ? accounts.OrderByDescending(a => a.Status)
                                  : accounts.OrderBy(a => a.Status),
                    _ => accounts
                };
            }

            //Count Total Data
            int totalData = await accounts.AsNoTracking().CountAsync();

            //Paging
            int skipNum = (getEmployeeRequest.PageNum - 1) * getEmployeeRequest.PageSize;
            accounts = accounts.Skip(skipNum).Take(getEmployeeRequest.PageSize);

            var result = await accounts.ToListAsync();

            int totalPage = (int)Math.Ceiling((decimal)totalData / getEmployeeRequest.PageSize);

            var pagination = new Pagination(totalData, getEmployeeRequest.PageSize, getEmployeeRequest.PageNum, totalPage);

            return (result, pagination);
        }
    }
}
