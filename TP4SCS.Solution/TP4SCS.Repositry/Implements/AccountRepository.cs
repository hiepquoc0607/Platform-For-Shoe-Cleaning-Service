using Microsoft.EntityFrameworkCore;
using TP4SCS.Library.Models.Data;
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
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            try
            {
                return await _dbConext.Accounts.Where(a => a.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Account>?> GetAccountsAsync()
        {
            try
            {
                return await _dbConext.Accounts.ToListAsync();
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
                return await _dbConext.Accounts.Where(a => a.Email.Equals(email)).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
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

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            try
            {
                return await _dbConext.Accounts.Where(a => a.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
