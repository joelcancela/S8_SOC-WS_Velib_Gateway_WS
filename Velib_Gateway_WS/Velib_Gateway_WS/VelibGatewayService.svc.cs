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
using System.Threading.Tasks;
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

        public async Task<string[]> GetCities()
        {
             return (await cache.getCitiesAsync()).ToArray();
        }

        public async Task<string[]> GetStations(string city)
        {
            return (await cache.getStationsAsync(city)).ToArray();
        }

        public int GetAvailableVelibs(string stationName) // NOTE: ne peut pas être appelée en premier
        {
            return cache.getVelibs(stationName);
        }
    }
}
