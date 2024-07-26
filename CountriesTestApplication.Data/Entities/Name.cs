using CountriesTestApplication.Data.Entities.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CountriesTestApplication.Data.Entities
{
    public class Name
    {
        public string Common { get; set; }
        public string Official { get; set; }
        public Dictionary<string, DetailName> NativeName { get; set; }
    }
}
