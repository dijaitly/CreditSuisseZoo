using CreditSuisse.QIS.Common;

namespace CreditSuisse.QIS.Interfaces
{
    public interface ICardService
    {
        bool Validate(string cardNumber, string pin);

        Card GetCardDetails(string cardNumber);
    }
}
