using AutoRegularInspection.IRepository;
using AutoRegularInspection.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Services
{
    public class NinjectDependencyResolver: Ninject.Modules.NinjectModule
    {
        public NinjectDependencyResolver()
        {
        }
        public override void Load()
        {
            Bind<IDataRepository>().To<ExcelDataRepository>();     
        }
    }
}
