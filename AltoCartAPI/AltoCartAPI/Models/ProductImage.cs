using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class ProductImage
    {

        public int Product_ID { get; set; }
        [ForeignKey("Product_ID")]
        public Product Product { get; set; }
        public int ID { get; set; }
        public string ProductImageURl { get; set; }

    }
}