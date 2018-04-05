using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleEventSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            VelibCallbackSink objsink = new VelibCallbackSink();
            InstanceContext iCntxt = new InstanceContext(objsink);
            VelibServiceSubReference.VelibServiceSubClient objClient = new VelibServiceSubReference.VelibServiceSubClient(iCntxt);
            objClient.SubscribeBikesNumber("pomme");
            Console.ReadLine();
        }
    }
}
