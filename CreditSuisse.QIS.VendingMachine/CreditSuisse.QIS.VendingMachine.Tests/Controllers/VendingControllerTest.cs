using CreditSuisse.QIS.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using CreditSuisse.QIS.VendingMachine.Controllers;
using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.VendingMachine.Tests.Controllers.Threading;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using CreditSuisse.QIS.VendingMachine.Tests.Mocks;

namespace CreditSuisse.QIS.VendingMachine.Tests.Controllers
{
    [TestClass]
    public class VendingControllerTest
    {
        [TestMethod]
        public void TestVendingWithIncorrectCardDetails()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();
            ICardService cardService = new CardService.CardService();
            VendingMachineController vmController = new VendingMachineController(cardService,accountService,vendingMachineService, new MockLogger());
            VendingRequest vRequest = new VendingRequest() { CardNumber = "1234", PIN = "2323223", NumberOfCans = 12 };
            IHttpActionResult response= vmController.Vend(vRequest);
            Assert.AreEqual(true, response is BadRequestErrorMessageResult);
            Assert.AreEqual("UnAuthorized", ((BadRequestErrorMessageResult)response).Message);
        }

        [TestMethod]
        public void TestVendingWithEmptyRequestDetails()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();
            ICardService cardService = new CardService.CardService();
            VendingMachineController vmController = new VendingMachineController(cardService, accountService, vendingMachineService, new MockLogger());
            VendingRequest vRequest = new VendingRequest() { CardNumber = "", PIN = "2345", NumberOfCans = 12 };
            IHttpActionResult response = vmController.Vend(vRequest);
            Assert.AreEqual(true, response is BadRequestErrorMessageResult);
            Assert.AreEqual("Invalid Request", ((BadRequestErrorMessageResult)response).Message);

            VendingRequest vRequest1 = new VendingRequest() { CardNumber = "1234", PIN = "", NumberOfCans = 12 };
            IHttpActionResult response1 = vmController.Vend(vRequest1);
            Assert.AreEqual(true, response1 is BadRequestErrorMessageResult);
            Assert.AreEqual("Invalid Request", ((BadRequestErrorMessageResult)response1).Message);

            VendingRequest vRequest2 = new VendingRequest() { CardNumber = "1234", PIN = "2323", NumberOfCans = 0 };
            IHttpActionResult response2 = vmController.Vend(vRequest2);
            Assert.AreEqual(true, response2 is BadRequestErrorMessageResult);
            Assert.AreEqual("Invalid Request", ((BadRequestErrorMessageResult)response2).Message);
        }

        [TestMethod]
        public void TestVendingWithCorrectDetails()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();
            ICardService cardService = new CardService.CardService();
            VendingMachineController vmController = new VendingMachineController(cardService, accountService, vendingMachineService, new MockLogger());
            VendingRequest vRequest = new VendingRequest() { CardNumber = "1234567", PIN = "1235", NumberOfCans = 2 };
            IHttpActionResult response = vmController.Vend(vRequest);
            Assert.AreEqual(true, response is OkResult);
        }


        [TestMethod]
        public void TestVendingWithCorrectDetailsMultipleRequest()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();
            
            List<Thread> threadList = new List<Thread>();
            List<RequestExecutor> requestExecutorList = new List<RequestExecutor>();
            for(int counter=0;counter<50;counter++)
            {
                VendingRequest vRequest = new VendingRequest() { CardNumber = "1234567", PIN = "1235", NumberOfCans = 2 };
                RequestExecutor requestExecutor = new RequestExecutor(vRequest,accountService,vendingMachineService);
                Thread t = new Thread(requestExecutor.Execute);
                threadList.Add(t);
                requestExecutorList.Add(requestExecutor);
            }

            foreach(Thread t1 in threadList)
            {
                t1.Start();
            }

            foreach(Thread t1 in threadList)
            {
                t1.Join();
            }

            var listofSuccess = requestExecutorList.Where(x => x.Response is OkResult);
            var listofFailure = requestExecutorList.Where(x => x.Response is BadRequestErrorMessageResult && ((BadRequestErrorMessageResult)x.Response).Message == "Not enough cans to dispense");
            Assert.AreEqual(12, listofSuccess.Count());
            Assert.AreEqual(38, listofFailure.Count());
        }


        [TestMethod]
        public void TestVendingWithCorrectDetailsForInsufficientFund()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();

            List<Thread> threadList = new List<Thread>();
            List<RequestExecutor> requestExecutorList = new List<RequestExecutor>();
            for (int counter = 0; counter < 50; counter++)
            {
                VendingRequest vRequest = new VendingRequest() { CardNumber = "75898256", PIN = "3321", NumberOfCans = 2 };
                RequestExecutor requestExecutor = new RequestExecutor(vRequest, accountService, vendingMachineService);
                Thread t = new Thread(requestExecutor.Execute);
                threadList.Add(t);
                requestExecutorList.Add(requestExecutor);
            }

            foreach (Thread t1 in threadList)
            {
                t1.Start();
            }

            foreach (Thread t1 in threadList)
            {
                t1.Join();
            }

            var listofSuccess = requestExecutorList.Where(x => x.Response is OkResult);
            var listofFailure = requestExecutorList.Where(x => x.Response is BadRequestErrorMessageResult && ((BadRequestErrorMessageResult)(x.Response)).Message  == "Not Sufficient Funds");
            Assert.AreEqual(2, listofSuccess.Count());
            Assert.AreEqual(48, listofFailure.Count());
        }

        [TestMethod]
        public void TestVendingWithCorrectDetailsForInsufficientFundWithAlternateCards()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();

            List<Thread> threadList = new List<Thread>();
            List<RequestExecutor> requestExecutorList = new List<RequestExecutor>();
            for (int counter = 0; counter < 50; counter++)
            {
                VendingRequest vRequest = null;
                if (counter % 2 == 0)
                {
                     vRequest = new VendingRequest() { CardNumber = "911258785", PIN = "1235", NumberOfCans = 2 };
                }
                else
                {
                     vRequest = new VendingRequest() { CardNumber = "587896369", PIN = "2435", NumberOfCans = 2 };
                }
                RequestExecutor requestExecutor = new RequestExecutor(vRequest, accountService, vendingMachineService);
                Thread t = new Thread(requestExecutor.Execute);
                threadList.Add(t);
                requestExecutorList.Add(requestExecutor);
            }

            foreach (Thread t1 in threadList)
            {
                t1.Start();
            }

            foreach (Thread t1 in threadList)
            {
                t1.Join();
            }

            var listofSuccess = requestExecutorList.Where(x => x.Response is OkResult);
            var listofFailure = requestExecutorList.Where(x => x.Response is BadRequestErrorMessageResult && ((BadRequestErrorMessageResult)(x.Response)).Message == "Not Sufficient Funds");
            Assert.AreEqual(2, listofSuccess.Count());
            Assert.AreEqual(48, listofFailure.Count());
        }

        [TestMethod]
        public void TestVendingWithCorrectDetailsMultipleRequestWithAlternateCards()
        {
            IAccountService accountService = new AccountService.AccountService();
            IVendingMachineService vendingMachineService = new VendingMachineService.VendingMachineService();

            List<Thread> threadList = new List<Thread>();
            List<RequestExecutor> requestExecutorList = new List<RequestExecutor>();
            for (int counter = 0; counter < 50; counter++)
            {
                VendingRequest vRequest = null;
                if (counter % 2 == 0)
                {
                    vRequest = new VendingRequest() { CardNumber = "1234567", PIN = "1235", NumberOfCans = 2 };
                }
                else
                {
                    vRequest = new VendingRequest() { CardNumber = "3456789", PIN = "2435", NumberOfCans = 2 };
                }
                RequestExecutor requestExecutor = new RequestExecutor(vRequest, accountService, vendingMachineService);
                Thread t = new Thread(requestExecutor.Execute);
                threadList.Add(t);
                requestExecutorList.Add(requestExecutor);
            }

            foreach (Thread t1 in threadList)
            {
                t1.Start();
            }

            foreach (Thread t1 in threadList)
            {
                t1.Join();
            }

            var listofSuccess = requestExecutorList.Where(x =>  x.Response is OkResult);
            var listofFailure = requestExecutorList.Where(x => x.Response is BadRequestErrorMessageResult && ((BadRequestErrorMessageResult)x.Response).Message == "Not enough cans to dispense");
            Assert.AreEqual(12, listofSuccess.Count());
            Assert.AreEqual(38, listofFailure.Count());
        }


    }
}
