using AltoCartAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.TemporaryModels
{
    public class TempOwner
    {

        public string OwnerName { get; set; }

        public string OwnerSurname { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public int Country_ID { get; set; }

        public int City_ID { get; set; }

        public string OwnerEmail { get; set; }

        public string Password { get; set; }

        public DateTime BirthDate { get; set; }


    }
}