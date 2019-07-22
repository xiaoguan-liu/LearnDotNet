using LDN.SelfWebServiceApp.WebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.SelfWebServiceApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketWebService socketWebService = new SocketWebService();
            socketWebService.Start();
        }
    }
}
