using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourAgency.DAL.Entities
{
    public class User: IdentityUser
    {
        public int? CityId { get; set; }
        [ForeignKey("CityId")]
        public virtual City City { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
        public User()
        {
            Tours = new HashSet<Tour>();
        }
    }
}
