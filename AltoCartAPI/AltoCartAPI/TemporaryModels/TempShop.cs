using AltoCartAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.TemporaryModels
{
    public class TempShop
    {

        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public string ShopImageUrl { get; set; }
        public int City_ID { get; set; }
        public int ShopCategory_ID { get; set; }
        public string FullAdress { get; set; }
        public string ShopEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
    }
}