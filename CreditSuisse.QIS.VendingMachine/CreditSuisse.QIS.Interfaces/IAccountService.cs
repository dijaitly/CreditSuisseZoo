using CreditSuisse.QIS.Common;

namespace CreditSuisse.QIS.Interfaces
{
    public interface IAccountService
    {
        ResponseMessage Deduct(string accountNumber,decimal value);
    }
}
