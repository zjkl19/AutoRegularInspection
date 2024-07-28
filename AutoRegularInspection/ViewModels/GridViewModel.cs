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
                DamageSummaryServices.InitListDamageSummary(lst, 2_000_000,bridgePart);    //尽管这里的2_000_000是被硬编码的，但实际生成的报告中，这个值是会被配置文件中的值替换
            }
            else
            {
                DamageSummaryServices.InitListDamageSummary(lst, 3_000_000, bridgePart);
            }
        
            GridSource.GridData = new ObservableCollection<DamageSummary>(lst);    // 使用 ObservableCollection 的构造函数直接转换 List

        }
        public GridModel GridSource { get; set; }
    }
}
