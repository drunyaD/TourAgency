using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourAgency.DAL.Interfaces;
using TourAgency.DAL.Repositories;

namespace TourAgency.BLL.Infrastructure
{   
    public class DalModule : NinjectModule
    {
         private readonly string _connectionString;
          public DalModule(string connection)
          {
              _connectionString = connection;
          }
          public override void Load()
          {
             Bind<IUnitOfWork>().To<UnitOfWork>().WithConstructorArgument(_connectionString);
          }
        }
    
}
