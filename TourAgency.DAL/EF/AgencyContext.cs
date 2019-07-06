using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.Entities;
using TourAgency.DAL.Identity;

namespace TourAgency.DAL.EF
{
    public class AgencyContext : IdentityDbContext<User>
    {
        public DbSet<City> Cities {get; set;}
        public DbSet<Country> Countries { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public AgencyContext(string connectionString) : base(connectionString)
        {
            InitializeDatabase();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        protected virtual void InitializeDatabase()
        {
            if (!Database.Exists())
            {
                Database.Initialize(true);
                new DatabaseInitializer().Seed(this);
            }
        }
    }

    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<AgencyContext>
    {
        public new void Seed(AgencyContext db)
        {
            var storeStore = new RoleStore<Role>(db);
            var roleManager = new AppRoleManager(storeStore);
            roleManager.Create(new Role() { Name = "user" });
            roleManager.Create(new Role() { Name = "moderator" });
            roleManager.Create(new Role() { Name = "administrator" });

            var userStore = new UserStore<User>(db);
            var userManager = new AppUserManager(userStore);
            User admin = new User
            {
                Email = "admin@mail.ru",
                UserName = "MainAdmin"
            };
            userManager.Create(admin, "adminpassword");
            userManager.AddToRole(admin.Id, "administrator");

            db.Countries.Add(new Country { Name = "Украина" });
            db.Countries.Add(new Country { Name = "Польша" });
            db.Countries.Add(new Country { Name = "Словакия" });
            db.Countries.Add(new Country { Name = "Чехия" });
            db.Countries.Add(new Country { Name = "Румыния" });

            db.Cities.Add(new City
            {
                Name = "Киев",
                Country = db.Countries
                    .Where(c => c.Name == "Украина").First()
            });

            db.Cities.Add(new City
            {
                Name = "Харьков",
                Country = db.Countries
                     .Where(c => c.Name == "Украина").First()
            });

            db.Cities.Add(new City
            {
                Name = "Львов",
                Country = db.Countries
                     .Where(c => c.Name == "Украина").First()
            });
            db.Cities.Add(new City
            {
                Name = "Полтава",
                Country = db.Countries
                     .Where(c => c.Name == "Украина").First()
            });
            db.Cities.Add(new City
            {
                Name = "Ивано-Франковск",
                Country = db.Countries
                     .Where(c => c.Name == "Украина").First()
            });
            db.Cities.Add(new City
            {
                Name = "Мукачево",
                Country = db.Countries
                     .Where(c => c.Name == "Украина").First()
            });

            db.Cities.Add(new City
            {
                Name = "Вроцлав",
                Country = db.Countries
                     .Where(c => c.Name == "Польша").First()
            });
            db.Cities.Add(new City
            {
                Name = "Варшава",
                Country = db.Countries
                     .Where(c => c.Name == "Польша").First()
            });

            db.Cities.Add(new City
            {
                Name = "Краков",
                Country = db.Countries
                     .Where(c => c.Name == "Польша").First()
            });
            db.Cities.Add(new City
            {
                Name = "Кошице",
                Country = db.Countries
                     .Where(c => c.Name == "Словакия").First()
            });
            db.Cities.Add(new City
            {
                Name = "Братислава",
                Country = db.Countries
                     .Where(c => c.Name == "Словакия").First()

            });
            db.Cities.Add(new City
            {
                Name = "Прага",
                Country = db.Countries
                     .Where(c => c.Name == "Чехия").First()
            });
            db.Cities.Add(new City
            {
                Name = "Карловы Вары",
                Country = db.Countries
                     .Where(c => c.Name == "Чехия").First()
            });
            db.Cities.Add(new City
            {
                Name = "Либерец",
                Country = db.Countries
                     .Where(c => c.Name == "Чехия").First()
            });
            db.Cities.Add(new City
            {
                Name = "Бухарест",
                Country = db.Countries
                     .Where(c => c.Name == "Румыния").First()
            });
            db.Cities.Add(new City
            {
                Name = "Яссы",
                Country = db.Countries
                     .Where(c => c.Name == "Румыния").First()
            }); 
            db.SaveChanges();
            base.Seed(db);
        }
    }
}
