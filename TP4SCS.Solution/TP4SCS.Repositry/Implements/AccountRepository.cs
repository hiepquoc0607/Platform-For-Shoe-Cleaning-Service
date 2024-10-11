using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
using TP4SCS.Library.Models.Response.Account;
using TP4SCS.Repository.Interfaces;

namespace TP4SCS.Repository.Implements
{
    public class AccountRepository : GenericRepoistory<Account>, IAccountRepository
    {
        private readonly Tp4scsDevDatabaseContext _dbConext;

        public AccountRepository(Tp4scsDevDatabaseContext dbConext) : base(dbConext)
        {
            _dbConext = dbConext;
        }

        public async Task CreateAccountAsync(Account account)
        {
            await InsertAsync(account);
            await SaveAsync();
        }

        public async Task<AccountResponse?> GetAccountByIdAsync(int id)
        {
            try
            {
                return await _dbConext.Accounts.Where(a => a.Id == id)
                    .Select(a => new AccountResponse
                    {
                        Id = a.Id,
                        Email = a.Email,
                        Password = a.Password,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Gender = a.Gender,
                        Dob = a.Dob,
                        ExpiredTime = a.ExpiredTime,
                        ImageUrl = a.ImageUrl,
                        IsGoogle = a.IsGoogle,
                        Fcmtoken = a.Fcmtoken,
                        Role = a.Role,
                        Status = a.Status
                    })
                    .FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<AccountResponse>?> GetAccountsAsync()
        {
            try
            {
                return await _dbConext.Accounts
                    .OrderBy(a => a.Status.Equals("ACTIVE"))
                    .ThenBy(a => a.Status.Equals("SUSPENDED"))
                    .ThenBy(a => a.Status.Equals("INACTIVE"))
                    .Select(a => new AccountResponse
                    {
                        Id = a.Id,
                        Email = a.Email,
                        Password = a.Password,
                        FullName = a.FullName,
                        Phone = a.Phone,
                        Gender = a.Gender,
                        Dob = a.Dob,
                        ExpiredTime = a.ExpiredTime,
                        ImageUrl = a.ImageUrl,
                        IsGoogle = a.IsGoogle,
                        Fcmtoken = a.Fcmtoken,
                        Role = a.Role,
                        Status = a.Status
                    })
                    .ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Account?> GetAccountByEmailAndPasswordAsync(string email, string password)
        {
            return await _dbConext.Accounts
                .Where(a => a.Email.Equals(email) && a.Password.Equals(password) && a.Status.Equals("ACTIVE"))
                .Select(a => new Account
                {
                    Id = a.Id,
                    Email = a.Email,
                    FullName = a.FullName,
                    Role = a.Role,
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsEmailExistedAsync(string email)
        {
            return await _dbConext.Accounts.AsNoTracking().AnyAsync(a => a.Email.Equals(email));
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await UpdateAsync(account);
        }

        public async Task<bool> IsPhoneExistedAsync(string phone)
        {
            return await _dbConext.Accounts.AsNoTracking().AnyAsync(a => a.Phone.Equals(phone));
        }

        public async Task<int> GetAccountMaxIdAsync()
        {
            return await _dbConext.Accounts.AsNoTracking().MaxAsync(a => a.Id);
        }
    }
}
