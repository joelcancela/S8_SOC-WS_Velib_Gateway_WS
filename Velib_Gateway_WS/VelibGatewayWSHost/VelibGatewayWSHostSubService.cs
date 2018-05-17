using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Velib_Gateway_WS;

namespace VelibGatewayWSHost
{
    class VelibGatewayWsHostSubService : IVelibServiceSub
    {
        //Events
        public static Dictionary<string, List<Action<int>>> subscribers = new Dictionary<string, List<Action<int>>>();

        public void SubscribeCalculatedEvent()
        {
        }

        public void SubscribeBikesNumber(string stationName)
        {
            Action<int> m_Bikes = delegate { };
            IVelibServiceSubEvents subscriber =
                OperationContext.Current.GetCallbackChannel<IVelibServiceSubEvents>();
            m_Bikes += subscriber.StationBikesNumberChanged;
            VelibGatewayWSHostService.cache.addSubscriber(m_Bikes, stationName);
        }

        public static void putSubscriber(string stationName, Action<int> action)
        {
            List<Action<int>> subscribersOfTheStation = null;
            if (subscribers.ContainsKey(stationName))
            {
                if (subscribers.TryGetValue(stationName, out subscribersOfTheStation))
                {
                    subscribersOfTheStation.Add(action);
                    subscribers[stationName] = subscribersOfTheStation;
                }
            }
            else
            {
                subscribersOfTheStation = new List<Action<int>>();
                subscribersOfTheStation.Add(action);
                subscribers.Add(stationName, subscribersOfTheStation);
            }

            Console.WriteLine("A client subscribed to " + stationName);
        }

        public static void triggerAllSubscribers(string stationName, int bikes)
        {
            List<Action<int>> subscribersOfTheStation = null;
            if (subscribers.ContainsKey(stationName))
            {
                if (subscribers.TryGetValue(stationName, out subscribersOfTheStation))
                {
                    foreach (Action<int> action in subscribersOfTheStation)
                    {
                        action(bikes);
                    }
                }
            }
        }
    }
}