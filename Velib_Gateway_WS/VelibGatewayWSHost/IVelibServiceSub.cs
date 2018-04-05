using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Velib_Gateway_WS
{
    [ServiceContract(CallbackContract = typeof(IVelibServiceSubEvents))]
    interface IVelibServiceSub
    {

        [OperationContract]
        void SubscribeBikesNumber(string stationName);
    }
}
