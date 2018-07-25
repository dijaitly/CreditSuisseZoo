using CreditSuisse.QIS.Interfaces;
using CreditSuisse.QIS.Logger;
using Unity;
using Unity.Lifetime;

namespace CreditSuisse.QIS.VendingMachine
{
    public class DependencyConfig
    {
        public static IUnityContainer Build()
        {
            var container = new UnityContainer();
            container.RegisterType<ICardService, CardService.CardService>();
            container.RegisterType<IAccountService, AccountService.AccountService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IVendingMachineService, VendingMachineService.VendingMachineService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILoggerInterface, CSLogger>(new ContainerControlledLifetimeManager());
            return container;
        }
    }
}