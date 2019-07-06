using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.Entities;

namespace TourAgency.DAL.Identity
{
   public class AppUserManager: UserManager<User>

    {
        public AppUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}
