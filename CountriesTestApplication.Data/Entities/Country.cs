using CountriesTestApplication.Data.Entities.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CountriesTestApplication.Data.Entities
{
    public class Country
    {
        public Name Name { get; set; }
        public List<double> LatLng { get; set; }
        public bool Landlocked { get; set; }
        public double Area { get; set; }
        public string cca2 { get; set; }
        public string ccn3 { get; set; }
        public string cca3 { get; set; }
        public string cioc { get; set; }
        public string Flag { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public int Population { get; set; }
        public List<string> Timezones { get; set; }
        public List<string> Continents { get; set; }
        public string StartOfWeek { get; set; }
        public string Region { get; set; }
        public string SubRegion { get; set; }
        public Dictionary<string, string> Languages { get; set; }
    }
}
