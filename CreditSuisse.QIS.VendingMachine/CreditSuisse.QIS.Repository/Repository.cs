using CreditSuisse.QIS.Common;
using CreditSuisse.QIS.Interfaces;
using System;
using System.Linq;

namespace CreditSuisse.QIS.Repository
{
    public class Repository : IRepository
    {
        private static object lockObject = new object();
        private static Repository repository;
        private CardAccount ca;
        
          
        private Repository()
        {
            Load();
        }

        private void Load()
        {
            ca = new CardAccount();
            CardAccount.AccountRow row = ca.Account.NewAccountRow();
            row.AccountNumber = "1234";
            row.Balance = "500";
            ca.Account.AddAccountRow(row);

            CardAccount.CardRow crow1 = ca.Card.NewCardRow();
            crow1.AccountNumber = "1234";
            crow1.CardNumber = "1234567";
            crow1.PIN = "1235";
            ca.Card.AddCardRow(crow1);

            CardAccount.CardRow crow2 = ca.Card.NewCardRow();
            crow2.AccountNumber = "1234";
            crow2.CardNumber = "3456789";
            crow2.PIN = "2435";
            ca.Card.AddCardRow(crow2);


            CardAccount.AccountRow row1 = ca.Account.NewAccountRow();
            row1.AccountNumber = "5678";
            row1.Balance = "2.50";
            ca.Account.AddAccountRow(row1);

            CardAccount.CardRow crow3 = ca.Card.NewCardRow();
            crow3.AccountNumber = "5678";
            crow3.CardNumber = "75898256";
            crow3.PIN = "3321";
            ca.Card.AddCardRow(crow3);

            CardAccount.CardRow crow4 = ca.Card.NewCardRow();
            crow4.AccountNumber = "5678";
            crow4.CardNumber = "47968582";
            crow4.PIN = "1258";
            ca.Card.AddCardRow(crow4);

            CardAccount.AccountRow row3 = ca.Account.NewAccountRow();
            row3.AccountNumber = "1357";
            row3.Balance = "2.50";
            ca.Account.AddAccountRow(row3);

            CardAccount.CardRow crow5 = ca.Card.NewCardRow();
            crow5.AccountNumber = "1357";
            crow5.CardNumber = "911258785";
            crow5.PIN = "1235";
            ca.Card.AddCardRow(crow5);

            CardAccount.CardRow crow6 = ca.Card.NewCardRow();
            crow6.AccountNumber = "1357";
            crow6.CardNumber = "587896369";
            crow6.PIN = "2435";
            ca.Card.AddCardRow(crow6);
        }

        public static Repository GetInstance()
        {
            lock (lockObject)
            {
                if (repository == null)
                {
                    repository = new Repository();
                }
            }
            return repository;
        }

        public ResponseMessage Deduct(string accountNumber,decimal value)
        {
            CardAccount.AccountRow aR = ca.Account.Where<CardAccount.AccountRow>(x => x.AccountNumber == accountNumber).SingleOrDefault();
            if(aR!=null)
            {
                decimal balance = Decimal.Parse(aR.Balance);
                if(balance < value)
                {
                    ResponseMessage message = new ResponseMessage();
                    message.IsSuccess = false;
                    message.ErrorMessage = "Not Sufficient Funds";
                    return message;
                }
                else
                {
                    balance = balance - value;
                    aR.Balance = balance.ToString();
                }
                
            }
            return new ResponseMessage() { IsSuccess = true };
        }

        public Card GetCard(string cardNumber)
        {
            CardAccount.CardRow cR = ca.Card.Where<CardAccount.CardRow>(x => x.CardNumber == cardNumber).SingleOrDefault();
            if (cR != null)
            {
                return new Card() { AccountNumber = cR.AccountNumber, PIN=cR.PIN,CardNumber=cR.CardNumber};
            }
            return null;
        }

    }
}
