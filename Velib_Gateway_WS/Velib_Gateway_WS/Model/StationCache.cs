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

        public StationCache()
        {
            this.cities = new List<String>();
            this.stations = new HashSet<Station>();
        }

        public List<String> getCities()
        {
            //TODO handle refresh update / attribute last update ?
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
        }


        public List<String> getStations(string city) //TODO handle refresh update
        {
            List<String> stationsName = new List<string>();
            //Insert control refresh here
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
        }

        internal int getVelibs(string stationName)//TODO handle update
        {
            foreach(Station station in stations){
                Console.WriteLine(station);
            }
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
            WebResponse response = request.GetResponse(); //Exception to handle
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