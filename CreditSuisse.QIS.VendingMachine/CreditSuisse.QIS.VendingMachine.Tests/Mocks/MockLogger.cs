using CreditSuisse.QIS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditSuisse.QIS.VendingMachine.Tests.Mocks
{
    class MockLogger : ILoggerInterface
    {
        public void LogException(Exception ex)
        {
            
        }
    }
}
