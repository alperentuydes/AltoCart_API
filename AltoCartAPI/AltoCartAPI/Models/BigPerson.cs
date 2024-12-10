using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace AltoCartAPI.Models
{
    public class BigPerson
    {

        public Guid GuidID { get; set; }
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted{ get; set; }
        public DateTime CreatedAt { get; set; }

        public BigPerson()
        {
            GuidID = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
            IsDeleted = false;
        }

    }
}