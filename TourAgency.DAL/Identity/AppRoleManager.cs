using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.Entities;

namespace TourAgency.DAL.Identity
{
   public class AppRoleManager: RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store) : base(store)
        {

        }
    }
}
