using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Velib_Gateway_WS.Model;

namespace Velib_Gateway_WS
{
    [ServiceContract]
    public interface IVelibService
    {

        [OperationContract]
        string[] GetStations(string city); // The client wants this

        [OperationContract]
        string[] GetCities(); // Implied by the previous

        [OperationContract]
        int GetAvailableVelibs(string stationName); // The client wants this
    }
}
