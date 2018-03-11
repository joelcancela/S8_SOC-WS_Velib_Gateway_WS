using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;
using System.Web.Script.Serialization;

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

        public StationCache()
        {
            this.cities = new List<String>();
            this.stations = new HashSet<Station>();
            this.lastCitiesUpdate = new DateTime();
            this.stationsUpdates = new Dictionary<string, DateTime>();
        }

        public List<String> getCities()
        {
            if (cities.Count == 0 || (DateTime.Now - lastCitiesUpdate).TotalSeconds > secondsToInvalidateCities)
            {
                System.Diagnostics.Debug.WriteLine("Updating cities list..."+DateTime.Now);
                updateCities();
            }
            return cities;
        }

        internal void updateCities()
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
        }


        public List<String> getStations(string city)
        {

            DateTime lastUpdate = new DateTime();
            stationsUpdates.TryGetValue(city, out lastUpdate);

            if (stations.Count == 0 || (DateTime.Now - lastUpdate).TotalSeconds > secondsToInvalidateStations)
            {
                System.Diagnostics.Debug.WriteLine("Updating stations for "+city+"..." + DateTime.Now);
                updateStations(city);
            }

            List<String> stationsName = new List<string>();
            List<Station> stationsOfCity = stations.Where(station => station.contract_name.Equals(city, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (Station station in stationsOfCity)
            {
                stationsName.Add(station.name);
            }
            return stationsName;
        }


        internal void updateStations(string city)
        {
            string result = CallRestService("https://api.jcdecaux.com/vls/v1/stations?contract=" + ToTitleCase(city) + "&apiKey=5cd3ee38df5bf19f1035644298031b4642bdcb4c");
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

        private string ToTitleCase(string s)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

    }
}