using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;
using CreditSuisse.QIS.VendingMachine.Controllers;
using CreditSuisse.QIS.VendingMachine.Tests.Mocks;
using System.Web.Http;

namespace CreditSuisse.QIS.VendingMachine.Tests.Controllers.Threading
{
    class RequestExecutor
    {
        private VendingRequest vRequest;
        private IHttpActionResult vResponse;
        private IAccountService accountService;
        private IVendingMachineService vendingMachineService;
        public IHttpActionResult Response { get { return vResponse; } }
        public RequestExecutor(VendingRequest vRequest,IAccountService accountService,IVendingMachineService vendingMachineService)
        {
            this.vRequest = vRequest;
            this.vendingMachineService = vendingMachineService;
            this.accountService = accountService;
        }
        

        public void Execute()
        {
            ICardService cardService = new CardService.CardService();
            VendingMachineController vmController = new VendingMachineController(cardService,accountService,vendingMachineService, new MockLogger());
            vResponse= vmController.Vend(vRequest);
        }
    }
}
