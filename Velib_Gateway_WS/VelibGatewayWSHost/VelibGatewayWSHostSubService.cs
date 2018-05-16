using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Velib_Gateway_WS;

namespace VelibGatewayWSHost
{
    class VelibGatewayWsHostSubService: IVelibServiceSub
    {
        //Events
        public static Action<int> m_Bikes;

        public void SubscribeCalculatedEvent()
        {
        }

        public void SubscribeBikesNumber(string stationName)
        {
            m_Bikes = delegate { };
            Console.WriteLine("A client subscribed to "+stationName);
            VelibGatewayWSHostService.cache.addSubscriber(m_Bikes,stationName);
            IVelibServiceSubEvents subscriber =
                OperationContext.Current.GetCallbackChannel<IVelibServiceSubEvents>();
            m_Bikes += subscriber.StationBikesNumberChanged;
        }
    }
}
