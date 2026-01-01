using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using Ninject;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void GenerateDamageStatisticsTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IKernel kernel = App.Kernel ?? new StandardKernel(new NinjectDependencyResolver());
                var dataRepository = kernel.Get<IDataRepository>();

                if (File.Exists($"{Path.GetFileName(App.DamageSummaryStatisticsFileName)}"))
                {
                    if (UserNotification.Confirm($"已存在{App.DamageSummaryStatisticsFileName}统计文件，是否覆盖？", "提示"))
                    {
                        File.Copy(App.DamageSummaryFileName, App.DamageSummaryStatisticsFileName, true);
                    }
                    else
                    {
                        return;
                    }

                }
                List<DamageSummary> lst;

                lst = dataRepository.ReadDamageData(BridgePart.BridgeDeck, App.DamageSummaryStatisticsFileName);
                DamageSummaryServices.InitListDamageSummary(lst);
                ObservableCollection<DamageSummary> oc = new ObservableCollection<DamageSummary>();
                lst.ForEach(x => oc.Add(x));
                ObservableCollection<DamageSummary> _bridgeDeckListDamageSummary = oc;

                lst = dataRepository.ReadDamageData(BridgePart.SuperSpace, App.DamageSummaryStatisticsFileName);
                DamageSummaryServices.InitListDamageSummary(lst, 2_000_000, BridgePart.SuperSpace);
                oc = new ObservableCollection<DamageSummary>();
                lst.ForEach(x => oc.Add(x));
                ObservableCollection<DamageSummary> _superSpaceListDamageSummary = oc;

                lst = dataRepository.ReadDamageData(BridgePart.SubSpace, App.DamageSummaryStatisticsFileName);
                DamageSummaryServices.InitListDamageSummary(lst, 3_000_000, BridgePart.SubSpace);
                oc = new ObservableCollection<DamageSummary>();
                lst.ForEach(x => oc.Add(x));
                ObservableCollection<DamageSummary> _subSpaceListDamageSummary = oc;

                DamageSummaryServices.GenerateDamageStatisticsTable(_bridgeDeckListDamageSummary, _superSpaceListDamageSummary, _subSpaceListDamageSummary);
                UserNotification.Info("成功生成桥梁检测病害统计汇总表！");
            }
            catch (Exception ex)
            {
                UserNotification.Error("生成统计表出错，请查看日志。", ex);
            }

        }

    }
}
