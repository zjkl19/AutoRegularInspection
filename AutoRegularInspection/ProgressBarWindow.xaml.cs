using AutoRegularInspection.Models;
using System;
using System.Collections.Generic;
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

namespace AutoRegularInspection
{
    /// <summary>
    /// ProgressBarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarWindow : Window
    {
        public ProgressBarWindow()
        {
            InitializeComponent();
        }
        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    // 获取 ProgressBarModel 实例并取消操作
        //    var model = this.DataContext as ProgressBarModel;
        //    model.CancellationTokenSource.Cancel();
        //}
        //private void Cancel_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("111");
        //    Close();        
        //}
    }
}
