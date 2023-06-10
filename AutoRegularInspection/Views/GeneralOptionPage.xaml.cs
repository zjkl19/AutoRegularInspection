using AutoRegularInspection.IRepository;
using System.Windows.Controls;

namespace AutoRegularInspection
{
    /// <summary>
    /// GeneralOptionPage.xaml 的交互逻辑
    /// </summary>
    public partial class GeneralOptionPage : UserControl, IOptionPage
    {
        public GeneralOptionPage()
        {
            InitializeComponent();
        }
        public string Title => "General";

        public UserControl Control => this;
    }
}
