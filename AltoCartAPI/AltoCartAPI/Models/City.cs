using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class City
    {

        public int Country_ID { get; set; }

        [ForeignKey("Country_ID")]
        public Country Country { get; set; }

        public int ID { get; set; }

        public string  CityName { get; set; }

        public ICollection<Shop> Shops { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Owner> Owners { get; set; }


    }
}