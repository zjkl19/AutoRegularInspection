using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using AutoRegularInspection;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoRegularInspection.ViewModels
{
    public class GridViewModel
    {
        public GridViewModel(BridgePart bridgePart=BridgePart.BridgeDeck)
        {
            GridSource = new GridModel();
            IKernel kernel = App.Kernel ?? new StandardKernel(new NinjectDependencyResolver());
            var dataRepository = kernel.Get<IDataRepository>();

            List<DamageSummary> lst;

            try
            {
                lst = dataRepository.ReadDamageData(bridgePart);
            }
            catch (DataValidationException ex)
            {
                var errorFile = "校验错误.txt";
                try
                {
                    File.WriteAllLines(errorFile, ex.Errors);
                }
                catch
                {
                    // ignore file write failures, still show popup
                }

                var preview = string.Join("\n", ex.Errors.Take(5));
                UserNotification.Error($"外观检查.xlsx 数据存在错误，共{ex.Errors.Count}条，已阻止加载。\n详情见 {errorFile}\n前几条：\n{preview}");
                throw;
            }
            catch (Exception ex)
            {
                UserNotification.Error("加载外观检查.xlsx 时发生异常，请检查数据或日志。", ex);
                throw;
            }

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
