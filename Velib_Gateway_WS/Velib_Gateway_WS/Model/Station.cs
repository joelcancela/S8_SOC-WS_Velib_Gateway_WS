using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Velib_Gateway_WS.Model
{
    public class Station : IComparable<Station>
    {
        public int number { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public bool banking { get; set; }
        public bool bonus { get; set; }
        public string status { get; set; }
        public string contract_name { get; set; }
        public int bike_stands { get; set; }
        public int available_bike_stands { get; set; }
        public int available_bikes { get; set; }
        public long last_update { get; set; }

        public int CompareTo(Station other)
        {
            return this.name.CompareTo(other.name);
        }

        public override bool Equals(object obj)
        {
            var station = obj as Station;
            return station != null &&
                   name == station.name;
        }

        public override int GetHashCode()
        {
            var hashCode = -1713175131;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            return hashCode;
        }
    }
}