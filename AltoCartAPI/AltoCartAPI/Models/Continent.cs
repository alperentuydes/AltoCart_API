using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Continent
    {

        public int ID { get; set; }
        public string ContinentName { get; set; }
        public ICollection<Country> Countries{ get; set; }

    }
}