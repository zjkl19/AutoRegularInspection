using AutoRegularInspection.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 启动时检查更新
        /// </summary>
        /// <remarks>
        /// 参考：https://www.cnblogs.com/huaxia283611/p/4185733.html
        /// </remarks>
        private void CheckForUpdateInStarup()
        {
            var appConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            bool autoApdate;
            try
            {
                autoApdate = Convert.ToBoolean(appConfig.AppSettings.Settings["AutoCheckForUpdate"].Value);
            }
            catch (Exception ex)
            {

#if DEBUG
                throw ex;

#else
                log.Error(ex, $"后台自动检查更新出错，错误信息：{ ex.Message.ToString()}");
                autoApdate = false;
#endif

            }


            if (autoApdate)
            {
                AutoCheckForUpdateCheckBox.IsChecked = true;
                //自动更新
                worker.WorkerReportsProgress = true;
                worker.DoWork += new DoWorkEventHandler(BackGroundCheckForUpdate);
                worker.ProgressChanged += BackGroundCheckForUpdate_ProgressChanged;
                worker.RunWorkerAsync();
            }
            else
            {
                AutoCheckForUpdateCheckBox.IsChecked = false;
            }
        }

        /// <summary>
        /// 进度返回处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BackGroundCheckForUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            StatusBarText.Text = (string)e.UserState;
        }


        void BackGroundCheckForUpdate(object sender, DoWorkEventArgs e)
        {
            worker.ReportProgress(0, "程序后台正在检查更新……");

            var resp = Repository.CheckForUpdate.GetRestResponse();

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                var v = resp.Content;

                var obtain = JsonConvert.DeserializeObject<GitHubLatestReleaseInfo>(v);    //TODO：增加异常处理

                if (obtain.tag_name != $"v{Application.ResourceAssembly.GetName().Version.ToString()}")
                {
                    if (MessageBox.Show($"检测到新版本{obtain.tag_name}\r更新说明：{obtain.body}\r是否下载新版本？", "检测到新版本", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        Process.Start(obtain.assets[0].browser_download_url);    //所有下载内容都打包到第1个assets
                    }
                }
                else
                {
                    //MessageBox.Show($"当前已是最新版本。");
                    worker.ReportProgress(0, "当前已是最新版本。");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                //MessageBox.Show($"获取更新信息失败。请检查网络是否通畅。Http状态码：{resp.StatusCode.ToString()}");
                worker.ReportProgress(0, $"获取更新信息失败。请检查网络是否通畅。Http状态码：{resp.StatusCode.ToString()}");
                Thread.Sleep(2000);
            }
            worker.ReportProgress(0, "就绪");

        }


    }
}
