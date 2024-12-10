using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Country
    {

        public int Continent_ID { get; set; }

        [ForeignKey("Continent_ID")]
        public Continent Continent { get; set; }
        public int ID { get; set; }
        public string CountryName { get; set; }
        public ICollection<City> Cities { get; set; }
        public ICollection<Owner> Owners { get; set; }


    }
}