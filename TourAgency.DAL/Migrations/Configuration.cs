namespace TourAgency.DAL.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TourAgency.DAL.Entities;
    using TourAgency.DAL.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<TourAgency.DAL.EF.AgencyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TourAgency.DAL.EF.AgencyContext db)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleStore = new RoleStore<Role>(db);
            var roleManager = new AppRoleManager(roleStore);
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

            db.SaveChanges();
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
