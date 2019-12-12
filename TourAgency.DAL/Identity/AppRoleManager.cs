using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TourAgency.DAL.Entities;

namespace TourAgency.DAL.Identity
{
    public class AppRoleManager : RoleManager<Role>
    {
        public AppRoleManager(RoleStore<Role> store) : base(store)
        {
        }
    }
}