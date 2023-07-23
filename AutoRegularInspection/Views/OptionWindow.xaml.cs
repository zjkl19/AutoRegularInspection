using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.ViewModels;
using NLog;
//using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
namespace AutoRegularInspection.Views
{
    /// <summary>
    /// OptionWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OptionWindow : Window
    {
        private ILogger _log;

        public OptionWindow()
        {
            InitializeComponent();
            OptionFrame.Tag = "Page1";
            //OptionContentControl.DataContext = new { SubPage = new Page1() };
        }

        public OptionWindow(ILogger log)
        {

            InitializeComponent();
            //OptionFrame.Tag = "Page1";
            //反序列化XML配置文件
            //var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OptionConfiguration));
            //StreamReader reader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");    //TODO：找不到文件的判断
            //var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);    //DataContext
            
            //OptionContentControl.DataContext = new { SubPage = new Page1() };
            _log = log;
        }

        private void OptionTreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //参考代码
            //var viewModel = (OptionViewModel)DataContext;
            //viewModel.SelectedOption = (Option)e.NewValue;
            var viewModel = (OptionViewModel)DataContext;
            //viewModel.SelectedOption = (Option)e.NewValue;
            viewModel.SelectedOption = (Option)e.NewValue;

        }
        
    }
}
