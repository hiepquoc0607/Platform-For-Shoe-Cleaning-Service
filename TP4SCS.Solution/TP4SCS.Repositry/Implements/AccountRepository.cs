using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Request.General;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class AccountRepository : GenericRepoistory<Account>, IAccountRepository
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
                return await _dbContext.Accounts.Where(a => a.Id == id && !a.Status.Equals("INACTIVE")).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Account>?> GetAccountsAsync(GetAccountRequest getAccountRequest)
        {
            try
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

                //Order Sort
                if (!string.IsNullOrEmpty(getAccountRequest.SortBy))
                {
                    accounts = getAccountRequest.SortBy.ToUpper() switch
                    {
                        "EMAIL" => getAccountRequest.IsDecsending
                                    ? accounts.OrderByDescending(a => a.Email)
                                    : accounts.OrderBy(a => a.Email),
                        "FULLNAME" => getAccountRequest.IsDecsending
                                      ? accounts.OrderByDescending(a => a.FullName)
                                      : accounts.OrderBy(a => a.FullName),
                        "STATUS" => getAccountRequest.IsDecsending
                                      ? accounts.OrderByDescending(a => a.Status)
                                      : accounts.OrderBy(a => a.Status),
                        _ => accounts
                    };
                }

                //Paging
                if (getAccountRequest.PageNum > 0 && getAccountRequest.PageSize > 0)
                {
                    int skipNum = (getAccountRequest.PageNum - 1) * getAccountRequest.PageSize;
                    accounts = accounts.Skip(skipNum).Take(getAccountRequest.PageSize);
                }

                return await accounts.Select(a => new Account
                {
                    Id = a.Id,
                    Email = a.Email,
                    FullName = a.FullName,
                    Phone = a.Phone,
                    Gender = a.Gender,
                    Dob = a.Dob,
                    ImageUrl = a.ImageUrl,
                    Role = a.Role,
                    Status = a.Status,
                })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<Account?> GetAccountLoginByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Accounts.Where(a => a.Email.Equals(email) && !a.Status.Equals("INACTIVE"))
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
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
                return await _dbContext.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
