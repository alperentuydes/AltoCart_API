using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class User
    {
        public int ID { get; set; }

        public string UserName { get; set; }

        public string UserSurname { get; set; }

        public string UserEmail { get; set; }

        public string UserPassword { get; set; }

        public int Country_ID { get; set; }

        public int City_ID { get; set; }

        [ForeignKey("City_ID")]
        public City City { get; set; }

        public string Adress { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime BirthDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}