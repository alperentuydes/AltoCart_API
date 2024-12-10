using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class ProductCategory
    {

        public int ID { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products{ get; set; }

    }
}