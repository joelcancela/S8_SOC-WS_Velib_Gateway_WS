using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using VelibGatewayWSHost;
using Timer = System.Timers.Timer;

namespace Velib_Gateway_WS.Model
{
    public class StationCache
    {
        private List<String> cities;
        private HashSet<Station> stations;
        private int secondsToInvalidateStations = 300; //5 minutes cooldown
        private int secondsToInvalidateCities = 3600; //1 hour
        private DateTime lastCitiesUpdate;
        private Dictionary<string, DateTime> stationsUpdates;
        //Events (only one client for now)
        private string stationSubscribe;
        private Timer timer;

        public StationCache()
        {
            this.cities = new List<String>();
            this.stations = new HashSet<Station>();
            this.lastCitiesUpdate = new DateTime();
            this.stationsUpdates = new Dictionary<string, DateTime>();
        }

        public async Task<List<string>> getCitiesAsync()
        {
            if (cities.Count == 0 || (DateTime.Now - lastCitiesUpdate).TotalSeconds >= secondsToInvalidateCities)
            {
                Debug.WriteLine("Updating cities list..."+DateTime.Now);
                await updateCities();
            }
            return cities;
        }

        internal async Task updateCities()
        {
            string result = await CallRestServiceAsync("https://api.jcdecaux.com/vls/v1/contracts?apiKey=5cd3ee38df5bf19f1035644298031b4642bdcb4c");
            JArray jArray = JArray.Parse(result);
            List<String> list = new List<string>();
            foreach (JObject json in jArray.Children().Where(child => ((string)child["name"]).Length > 0).ToList())
            {
                list.Add(json["name"].ToString());
            }
            list.Sort();
            cities = list;
            lastCitiesUpdate = DateTime.Now;
        }


        public async Task<List<string>> getStationsAsync(string city)
        {

            DateTime lastUpdate = new DateTime();
            stationsUpdates.TryGetValue(city, out lastUpdate);

            if (stations.Count == 0 || (DateTime.Now - lastUpdate).TotalSeconds >= secondsToInvalidateStations)
            {
                Debug.WriteLine("Updating stations for "+city+"..." + DateTime.Now);
                await updateStations(city);
            }

            List<String> stationsName = new List<string>();
            List<Station> stationsOfCity = stations.Where(station => station.contract_name.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (Station station in stationsOfCity)
            {
                stationsName.Add(station.name);
            }
            return stationsName;
        }


        internal async Task updateStations(string city)
        {
            string result = await CallRestServiceAsync("https://api.jcdecaux.com/vls/v1/stations?contract=" + ToTitleCase(city) + "&apiKey=5cd3ee38df5bf19f1035644298031b4642bdcb4c");
            List<Station> stationsCity = new JavaScriptSerializer().Deserialize<List<Station>>(result);
            stationsCity.Sort();
            stations.UnionWith(stationsCity);
            stationsUpdates[city]= DateTime.Now;
        }

        internal int getVelibs(string stationName)
        {
            Station stationToGet = stations.Where(station => station.name.Contains(stationName.ToUpper())).FirstOrDefault();
            if(stationToGet == null)
            {
                throw new FaultException("Station inconnue ou non repertoriée, essayez de rechercher la ville correspondante d'abord ou changez le paramètre <stationName>");
            }
            else
            {
                return stationToGet.available_bikes;
            }
        }


        private string CallRestService(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            return reader.ReadToEnd();
        }

        private async Task<string> CallRestServiceAsync(string url)
        {
            var myTask = Task.Factory.StartNew(() => CallRestService(url));
            var result = await myTask;
            return result;
        }

        private string ToTitleCase(string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public void addSubscriber(Action<int> mBikes, string stationName)
        {
            if (timer!=null)
            {
                timer.Stop();
            }
            Station stationopt = stations.Where(station => station.name.Contains(stationName.ToUpper()))
                .FirstOrDefault();
            if (stationopt != null)
            {
                timer = new Timer(3000);
                timer.Elapsed += delegate { OnElapsed(stationopt); };
                timer.AutoReset = false;
                timer.Start();
            }
        }

        private void OnElapsed(Station station)
        {
            VelibGatewayWsHostSubService.m_Bikes(station.available_bikes);
        
            timer.Start();
        }

        public string getStation(string stationName)
        {
            string stationOpt = stations.Where(station => station.name.Contains(stationName.ToUpper())).FirstOrDefault()
                .name;
            if (stationOpt == null)
            {
                return " ";
            }
            return stationOpt;
        }

        public void initCityAndStations()
        {

            string result = CallRestService("https://api.jcdecaux.com/vls/v1/contracts?apiKey=5cd3ee38df5bf19f1035644298031b4642bdcb4c");
            JArray jArray = JArray.Parse(result);
            List<String> list = new List<string>();
            foreach (JObject json in jArray.Children().Where(child => ((string)child["name"]).Length > 0).ToList())
            {
                list.Add(json["name"].ToString());
            }
            list.Sort();
            cities = list;
            lastCitiesUpdate = DateTime.Now;


            foreach (string city in cities)
            {
                result = CallRestService("https://api.jcdecaux.com/vls/v1/stations?contract=" + ToTitleCase(city) + "&apiKey=5cd3ee38df5bf19f1035644298031b4642bdcb4c");
                List<Station> stationsCity = new JavaScriptSerializer().Deserialize<List<Station>>(result);
                stationsCity.Sort();
                stations.UnionWith(stationsCity);
                stationsUpdates[city] = DateTime.Now;
            }




        }
    }
}