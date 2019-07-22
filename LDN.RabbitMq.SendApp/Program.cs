using LDN.RabbitMq.SendApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDN.RabbitMq.SendApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region HelloWorld
            //HelloWorld helloWorld = new HelloWorld();
            //helloWorld.Start(); 
            #endregion

            #region WorkQueues
            WorkQueues workQueues = new WorkQueues();
            workQueues.Start();
            #endregion
        }
    }
}
