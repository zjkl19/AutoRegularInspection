using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoRegularInspection.Models;
using AutoRegularInspection.Services;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void ValidatePictures_Click(object sender, RoutedEventArgs e)
        {
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

            List<DamageSummary> l1 = _bridgeDeckListDamageSummary.ToList();
            List<DamageSummary> l2 = _superSpaceListDamageSummary.ToList();
            List<DamageSummary> l3 = _subSpaceListDamageSummary.ToList();

            DamageSummaryServices.InitListDamageSummary1(l1);
            DamageSummaryServices.InitListDamageSummary1(l2, 2_000_000);
            DamageSummaryServices.InitListDamageSummary1(l3, 3_000_000);

            int totalInvalidPictureCounts = PictureServices.ValidatePictures(l1, l2, l3, out List<string> bridgeDeckValidationResult, out List<string> superSpaceValidationResult, out List<string> subSpaceValidationResult);
            try
            {
                WriteInvalidPicturesResultToTxt(totalInvalidPictureCounts, bridgeDeckValidationResult, superSpaceValidationResult, subSpaceValidationResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }

            MessageBoxResult k = MessageBox.Show($"照片验证完成！其中无效照片共计{totalInvalidPictureCounts}张，结果详见根目录文件“无效照片列表.txt”");

        }

        private static void WriteInvalidPicturesResultToTxt(int totalInvalidPictureCounts, List<string> bridgeDeckValidationResult, List<string> superSpaceValidationResult, List<string> subSpaceValidationResult)
        {
            string storeFile = App.InvalidPicturesStoreFile;
            FileStream stream;
            if (!File.Exists(storeFile))
            {
                _ = File.Create(storeFile);

            }
            stream = new FileStream(storeFile, FileMode.Append);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine($"当前时间：{DateTime.Now}");
            writer.WriteLine($"共计{totalInvalidPictureCounts}张照片无效。");

            for (int i = 0; i < bridgeDeckValidationResult.Count; i++)
            {
                writer.WriteLine(bridgeDeckValidationResult[i]);
            }
            for (int i = 0; i < superSpaceValidationResult.Count; i++)
            {
                writer.WriteLine(superSpaceValidationResult[i]);
            }
            for (int i = 0; i < subSpaceValidationResult.Count; i++)
            {
                writer.WriteLine(subSpaceValidationResult[i]);
            }
            writer.Close();
            stream.Close();
        }

       
    }
}
