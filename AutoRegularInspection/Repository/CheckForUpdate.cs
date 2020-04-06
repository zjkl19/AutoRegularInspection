using AutoRegularInspection.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;

namespace AutoRegularInspection.Repository
{
    public static class CheckForUpdate
    {
        public static readonly string url = @"https://api.github.com/";

        //TODO:单元测试

        public static void CheckByRestClient()
        {
            //参考https://blog.csdn.net/Cjiaocsda1127/article/details/82765423
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new RestClient(url);

            var request = new RestRequest($"repos/zjkl19/AutoRegularInspection/releases/latest", Method.GET);

            try
            {
                var resp = client.Execute(request);

                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var v = resp.Content;

                    var obtain = JsonConvert.DeserializeObject<GitHubLatestReleaseInfo>(v);    //TODO：增加异常处理

                    //MessageBox.Show($"获取成功! 内容：{obtain.tag_name}");
                    if (obtain.tag_name != $"v{Application.ResourceAssembly.GetName().Version.ToString()}")
                    {
                        if (MessageBox.Show($"检测到新版本{obtain.tag_name}\r更新说明：{obtain.body}\r是否下载新版本？", "检测到新版本", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            System.Diagnostics.Process.Start(obtain.assets[0].browser_download_url);    //所有下载内容都打包到第1个assets
                        }
                    }
                    else
                    {
                        MessageBox.Show($"当前已是最新版本。");
                    }
                }
                else
                {
                    MessageBox.Show($"获取更新信息失败。请检查网络是否通畅。Http状态码：{resp.StatusCode.ToString()}");
                }

            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                MessageBox.Show($"检查更新出错，错误码：{ex.Message.ToString()}");
#endif
            }
        }

        public static IRestResponse GetRestResponse()
        {
            //参考https://blog.csdn.net/Cjiaocsda1127/article/details/82765423
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new RestClient(url);

            var request = new RestRequest($"repos/zjkl19/AutoRegularInspection/releases/latest", Method.GET);

            try
            {
                return client.Execute(request);
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                //TODO：log
                return null;
#endif
            }
        }
    }
}
