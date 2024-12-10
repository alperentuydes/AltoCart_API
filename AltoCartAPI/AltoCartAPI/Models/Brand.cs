using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Brand
    {

        public int ID { get; set; }
        public string BrandName { get; set; }
        public ICollection<Product> Product { get; set; }

    }
}