namespace CreditSuisse.QIS.VendingMachine.Helpers
{
    public class RequestValidationHelper
    {
        public static bool Validate(string cardNumber,string pin, int numberOfCans)
        {
            if(string.IsNullOrEmpty(cardNumber))
            {
                return false;
            }

            if(string.IsNullOrEmpty(pin))
            {
                return false;
            }

            if(numberOfCans <=0)
            {
                return false;
            }
            return true;
        }
    }
}