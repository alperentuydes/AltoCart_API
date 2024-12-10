using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Owner
    {
        public int? Shop_ID { get; set; }

        [ForeignKey("Shop_ID")]
        public Shop Shop { get; set; }

        public string OwnersShopName { get; set; }

        public Guid Guid_ID { get; set; }
        public int ID { get; set; }

        public string OwnerName { get; set; }

        public string OwnerSurname { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OwnerEmail { get; set; }

        public string PasswordHash { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Country_ID { get; set; }

        [ForeignKey("Country_ID")]
        public Country Countries { get; set; }

        public string OwnersCountry { get; set; }

        public int City_ID { get; set; }

        [ForeignKey("City_ID")]
        public City Cities { get; set; }
        public string OwnersCity { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

    }
}