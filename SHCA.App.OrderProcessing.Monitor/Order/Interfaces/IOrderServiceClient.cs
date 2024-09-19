using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.App.OrderProcessing.Monitor.Interfaces
{

    public interface IOrderServiceClient
    {
        Task<string?> FetchMedicalEquipmentOrders();
    }
}