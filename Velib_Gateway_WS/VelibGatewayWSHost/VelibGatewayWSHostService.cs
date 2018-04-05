using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Velib_Gateway_WS;
using Velib_Gateway_WS.Model;

namespace VelibGatewayWSHost
{
    class VelibGatewayWSHostService : IVelibService
    {
        public static StationCache cache;

        private VelibGatewayWSHostService()
        {
            if (cache == null)
            {
                cache = new StationCache();
                cache.initCityAndStations();
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

        public async Task<int> GetAvailableVelibs(string stationName) // NOTE: ne peut pas être appelée en premier
        {
            return await Task.Run(() => cache.getVelibs(stationName));
        }
    }
}
