using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class ShopCategory
    {

        public int ID { get; set; }
        public string ShopCategoryName { get; set; }
        public string ShopCategoryDescription { get; set; }
        public ICollection<Shop> Shops { get; set; }

    }
}