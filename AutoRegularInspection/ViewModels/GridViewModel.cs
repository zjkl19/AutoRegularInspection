using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.ViewModels
{
    public class GridViewModel
    {
        public GridViewModel(BridgePart bridgePart=BridgePart.BridgeDeck)
        {
            GridSource = new GridModel();
            IKernel kernel = new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();


            List<DamageSummary> lst;

            lst = dataRepository.ReadDamageData(bridgePart);
            
            //DamageSummaryServices.InitListDamageSummary(lst);

            ObservableCollection<DamageSummary> oc = new ObservableCollection<DamageSummary>();

            lst.ForEach(x => oc.Add(x));
            foreach(var k in oc)
            {
                GridSource.GridData.Add(k);
            }
            
        }
        public GridModel GridSource { get; set; }
    }
}
