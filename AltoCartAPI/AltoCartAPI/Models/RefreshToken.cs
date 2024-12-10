using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class RefreshToken
    {

        public int ID { get; set; }
        public string Token { get; set; }
        public Guid UserGuid { get; set; }
        public DateTime ExpireDate { get; set; }

    }
}