using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Shop
    {

        public int ID { get; set; }

        public string ShopName { get; set; }

        public string ShopDescription { get; set; }
        public string ShopImageUrl { get; set; }

        public int City_ID { get; set; }

        [ForeignKey("City_ID")]
        public City City { get; set; }

        public int ShopCategory_ID { get; set; }

        [ForeignKey("ShopCategory_ID")]
        public ShopCategory ShopCategory { get; set; }

        public ICollection<Owner> Owners { get; set; }

        public ICollection<Worker> Workers { get; set; }

        public ICollection<Product> Products { get; set; }

        public bool IsApprovedByBigPerson { get; set; }

        public string ApprovedByWho { get; set; }

        public DateTime ApprovedDate { get; set; }

        public string FullAdress { get; set; }

        public string ShopEmail { get; set; }

        public string PhoneNumber { get; set; }

        public string PostalCode { get; set; }

        public float ShopRating { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}