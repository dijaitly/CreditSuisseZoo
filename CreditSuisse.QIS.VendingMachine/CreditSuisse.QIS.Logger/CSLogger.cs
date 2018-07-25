using CreditSuisse.QIS.Interfaces;
using System;
using NLog;

namespace CreditSuisse.QIS.Logger
{
    public class CSLogger : ILoggerInterface
    {

        private  ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogException(Exception ex)
        {
            logger.Error(ex);
        }
    }
}
