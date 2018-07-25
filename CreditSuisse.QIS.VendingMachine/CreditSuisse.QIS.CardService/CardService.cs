using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;

namespace CreditSuisse.QIS.CardService
{
    public class CardService : ICardService
    {
        private IRepository repository;

        public CardService()
        {
            repository = Repository.Repository.GetInstance();
        }
    
        public Card GetCardDetails(string cardNumber)
        {
            if (!string.IsNullOrEmpty(cardNumber))
            {
                return repository.GetCard(cardNumber);
            }

            return null;
        }

        public bool Validate(string cardNumber, string pin)
        {
            if (!string.IsNullOrEmpty(cardNumber) && !string.IsNullOrEmpty(pin))
            {
                var card = repository.GetCard(cardNumber);
                if (card != null && card.PIN == pin)
                {
                    return true;
                }
            }
            return false;

        }

        
    }
}
