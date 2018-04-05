using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace VelibGatewayWSHost
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create ServiceHost
            ServiceHost host = new ServiceHost(typeof(VelibGatewayWSHostService));
            ServiceHost host2 = new ServiceHost(typeof(VelibGatewayWsHostSubService));
            //Start the Service
            host.Open();
            host2.Open();
            Console.WriteLine("Service is host at " + DateTime.Now.ToString());
            Console.WriteLine("Host is running... Press <Enter> key to stop");
            Console.ReadLine();
            host2.Close();
            host.Close();
        }
    }
}
