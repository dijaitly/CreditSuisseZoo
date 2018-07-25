using CreditSuisse.QIS.Common;

namespace CreditSuisse.QIS.Interfaces
{
    public interface IRepository
    {
        Card GetCard(string cardNumber);
        ResponseMessage Deduct(string accountNumber,decimal value);
    }
}
