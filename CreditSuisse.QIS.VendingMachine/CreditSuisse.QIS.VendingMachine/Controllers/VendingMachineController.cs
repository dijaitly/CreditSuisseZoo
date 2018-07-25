using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;
using CreditSuisse.QIS.VendingMachine.Helpers;
using System;
using System.Web.Http;

namespace CreditSuisse.QIS.VendingMachine.Controllers
{
    public class VendingMachineController : ApiController
    {
        private ICardService cardService;
        private IAccountService accountService;
        private IVendingMachineService vendingMachineService;
        private ILoggerInterface logger;
        public VendingMachineController(ICardService cardService,IAccountService accountService,IVendingMachineService service,ILoggerInterface logger)
        {
            this.cardService = cardService;
            this.accountService = accountService;
            this.vendingMachineService = service;
            this.logger = logger;
        }
       // POST api/<controller>

       [HttpPost]
       [Route("api/VendingMachine/Vend")]
        public IHttpActionResult Vend(VendingRequest request)
        {
            string response = string.Empty;
            try
            {
                if (RequestValidationHelper.Validate(request.CardNumber, request.PIN, request.NumberOfCans))
                {
                    if (cardService.Validate(request.CardNumber, request.PIN))
                    {
                        Card card = cardService.GetCardDetails(request.CardNumber);
                        if (card != null && !string.IsNullOrEmpty(card.AccountNumber))
                        {
                            VendActionManager manager = VendActionManager.GetInstance();
                            var message = manager.Vend(vendingMachineService, accountService, request.NumberOfCans, card.AccountNumber);
                            if (message != null && message.IsSuccess)
                                response = "Success";
                            else
                                response = message != null ? message.ErrorMessage : "Error Occured";
                        }
                        else
                        {
                            response = "Account Not found";
                        }
                    }
                    else
                    {
                        response = "UnAuthorized";
                    }
                }
                else
                {
                    response = "Invalid Request";
                }

                if (response == "Success")
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch(Exception ex)
            {
                logger.LogException(ex);
                return InternalServerError();
            }
        }
    }
}