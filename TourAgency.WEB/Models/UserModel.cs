using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourAgency.WEB.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? CityId { get; set; }
        public string Role { get; set; }
    }
}