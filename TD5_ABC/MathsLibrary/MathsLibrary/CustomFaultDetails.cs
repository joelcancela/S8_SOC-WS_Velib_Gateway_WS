using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MathsLibrary
{
    [DataContract]
    public class CustomFaultDetails
    {
        public CustomFaultDetails(string message)
        {
            Message = message;
        }

        [DataMember]
        public string Message { get; set; }
    }
}
