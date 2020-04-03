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
            
            if(bridgePart==BridgePart.BridgeDeck)
            { 
                DamageSummaryServices.InitListDamageSummary(lst);
            }
            else if(bridgePart == BridgePart.SuperSpace)
            {
                DamageSummaryServices.InitListDamageSummary(lst, 2_000_000,bridgePart);
            }
            else
            {
                DamageSummaryServices.InitListDamageSummary(lst, 3_000_000, bridgePart);
            }
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
