using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourAgency.DAL.Entities
{
    public class User : IdentityUser
    {
        public int? CityId { get; set; }
        [ForeignKey("CityId")] public virtual City City { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }

        public User()
        {
            Tours = new HashSet<Tour>();
        }
    }
}