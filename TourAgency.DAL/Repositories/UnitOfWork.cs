using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.EF;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Identity;
using TourAgency.DAL.Interfaces;

namespace TourAgency.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly AgencyContext _db;
        private Repository<City> _cityRepository;
        private Repository<Tour> _tourRepository;
        private Repository<Image> _imageRepository;
        private Repository<Country> _countryRepository;
        private Repository<Node> _nodeRepository;
        private AppUserManager _userManager;
        private AppRoleManager _roleManager;

        public UnitOfWork(string connectionString)
        {
            _db = new AgencyContext(connectionString);
        }

        public IRepository<City> Cities =>
            _cityRepository ?? (_cityRepository = new Repository<City>(_db));

        public IRepository<Country> Countries =>
            _countryRepository ?? (_countryRepository = new Repository<Country>(_db));

        public IRepository<Node> Nodes =>
            _nodeRepository ?? (_nodeRepository = new Repository<Node>(_db));

        public IRepository<Tour> Tours =>
            _tourRepository ?? (_tourRepository = new Repository<Tour>(_db));

        public IRepository<Image> Images =>
            _imageRepository ?? (_imageRepository = new Repository<Image>(_db));


        public AppUserManager UserManager =>
            _userManager ?? (_userManager = new AppUserManager(new UserStore<User>(_db)));

        public AppRoleManager RoleManager =>
            _roleManager ?? (_roleManager = new AppRoleManager(new RoleStore<Role>(_db)));

        public void Save()
        {
           
           _db.SaveChanges();         
          
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                if (_db != null) _db.Dispose();
                if (_userManager != null) _userManager.Dispose();
                if (_roleManager != null) _roleManager.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
