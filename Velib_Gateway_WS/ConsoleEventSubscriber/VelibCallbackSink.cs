using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEventSubscriber
{
    class VelibCallbackSink  : VelibServiceSubReference.IVelibServiceSubCallback
    {

        public void StationBikesNumberChanged(int value)
        {
            Console.WriteLine("Bikes available "+value);
        }
    }
}
