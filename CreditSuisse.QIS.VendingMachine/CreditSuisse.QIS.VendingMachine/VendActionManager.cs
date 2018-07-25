using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;

namespace CreditSuisse.QIS.VendingMachine
{
    internal class VendActionManager
    {
        private static object lockObject = new object();
        private static VendActionManager vendActionManager;
        private VendActionManager()
        {

        }

        internal static VendActionManager GetInstance()
        {
            lock (lockObject)
            {
                if (vendActionManager == null)
                {
                    vendActionManager = new VendActionManager();
                }
                
            }
            return vendActionManager;
        }

        internal ResponseMessage Vend(IVendingMachineService vendingMachineService,IAccountService accountService,int numberOfCans,string accountNumber)
        {
            ResponseMessage message = null;
            lock (lockObject)
            {
                message= accountService.Deduct(accountNumber,numberOfCans *0.5m);
                if (message != null && message.IsSuccess)
                {
                    message = vendingMachineService.Vend(numberOfCans);
                }
            }
            return message;
        }
    }
}