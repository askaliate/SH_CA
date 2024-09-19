
using Microsoft.Identity.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHCA.App.OrderProcessing.Monitor.Auth
{
    public interface ITokenHandler
    {
        Task<string> GetTokenAsync();
    }
}
