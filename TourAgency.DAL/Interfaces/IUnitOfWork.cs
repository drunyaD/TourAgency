using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Identity;

namespace TourAgency.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        AppUserManager UserManager { get; }
        AppRoleManager RoleManager { get; }
        IRepository<City> Cities { get; }
        IRepository<Country> Countries { get; }
        IRepository<Node> Nodes { get; }
        IRepository<Image> Images { get; }
        IRepository<Tour> Tours { get; }
        void Save();
    }
}
