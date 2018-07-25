using CreditSuisse.QIS.Common;

namespace CreditSuisse.QIS.Interfaces
{
    public interface IVendingMachineService
    {
        ResponseMessage Vend(int numberOfCans);
    }
}
