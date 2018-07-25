using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;


namespace CreditSuisse.QIS.VendingMachineService
{
    public class VendingMachineService : IVendingMachineService
    {
        private int numberOfCansInMachine = 25;

        public ResponseMessage Vend(int numberOfCans)
        {
            if (numberOfCans <= numberOfCansInMachine)
            {
                numberOfCansInMachine = numberOfCansInMachine - numberOfCans;
                return new ResponseMessage() { IsSuccess = true };
            }
            return new ResponseMessage() { IsSuccess = false, ErrorMessage = "Not enough cans to dispense" };
        }
    }
}
