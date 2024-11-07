using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
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
                return await _dbContext.Accounts.Where(a => a.Id == id).SingleOrDefaultAsync();
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

            //Owner Sort
            if (getAccountRequest.OwnerId != null)
            {
                accounts = accounts.Where(a => a.CreatedByOwnerId == getAccountRequest.OwnerId);
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
                return await _dbContext.Accounts.Where(a => a.Email.Equals(email)).AsNoTracking().SingleOrDefaultAsync();
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
                return await _dbContext.Accounts.Where(a => a.Email.Equals(email)).FirstOrDefaultAsync();
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
                return await _dbContext.Accounts.Where(a => a.Id == id).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
