using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Product
    {
        public int Shop_ID { get; set; }

        [ForeignKey("Shop_ID")]
        public Shop Shop { get; set; }

        public int ProductCategory_ID { get; set; }

        [ForeignKey("ProductCategory_ID")]
        public ProductCategory ProductCategory { get; set; }

        public int ID { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public ICollection<ProductImage> ProductImage { get; set; }

        public int Discount { get; set; }

        public int Quantity { get; set; }

        public int Brand_ID { get; set; }

        [ForeignKey("Brand_ID")]
        public Brand Brand { get; set; }


    }
}