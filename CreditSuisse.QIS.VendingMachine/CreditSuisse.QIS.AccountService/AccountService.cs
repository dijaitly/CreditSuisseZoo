using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;

namespace CreditSuisse.QIS.AccountService
{
    public class AccountService : IAccountService
    {
        private IRepository repository;
        public AccountService()
        {
            this.repository = Repository.Repository.GetInstance();
        }
        public ResponseMessage Deduct(string accountNumber,decimal value)
        {
            return repository.Deduct(accountNumber, value);
        }

       
    }
}
