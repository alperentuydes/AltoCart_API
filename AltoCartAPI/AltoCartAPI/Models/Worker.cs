using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AltoCartAPI.Models
{
    public class Worker
    {

        public int Shop_ID { get; set; }

        [ForeignKey("Shop_ID")]
        public Shop Shop { get; set; }

        public int ID { get; set; }

        public string WorkerName { get; set; }

        public string WorkerSurname { get; set; }

        public string WorkerEmail { get; set; }

        public string WorkerPhoneNumber { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}