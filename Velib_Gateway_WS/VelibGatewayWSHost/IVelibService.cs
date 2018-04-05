using System.ServiceModel;
using System.Threading.Tasks;

namespace Velib_Gateway_WS
{
    [ServiceContract]
    public interface IVelibService
    {

        [OperationContract]
        Task<string[]> GetStations(string city); // The client wants this

        [OperationContract]
        Task<string[]> GetCities(); // Implied by the previous

        [OperationContract]
        Task<int> GetAvailableVelibs(string stationName); // The client wants this
    }
}
