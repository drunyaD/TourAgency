using Microsoft.AspNet.Identity;
using TourAgency.DAL.Entities;

namespace TourAgency.DAL.Identity
{
    public class AppUserManager : UserManager<User>

    {
        public AppUserManager(IUserStore<User> store) : base(store)
        {
        }
    }
}