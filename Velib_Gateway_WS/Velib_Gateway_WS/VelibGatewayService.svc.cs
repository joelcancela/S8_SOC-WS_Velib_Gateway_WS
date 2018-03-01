using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Velib_Gateway_WS.Model;

namespace Velib_Gateway_WS
{
   
    public class VelibGatewayService : IVelibService
    {
        private static StationCache cache;

        private VelibGatewayService()
        {
            if(cache == null)
            {
                cache = new StationCache();
            }
           
        }

        public string[] GetCities()
        {
            List<String> cities = cache.getCities();
            if(cities.Count == 0)
            {
                cache.updateCities();
                cities = cache.getCities();
            }
             return cities.ToArray();
        }

        public string[] GetStations(string city)
        {
            List<String> stations = cache.getStations(city);
            if(stations.Count == 0)
            {
                cache.updateStations(city);
                stations = cache.getStations(city);
            }
            return stations.ToArray();
        }

        public int GetAvailableVelibs(string stationName) // NOTE: ne peut pas être appelée en premier
        {
            return cache.getVelibs(stationName);
        }
    }
}
