﻿using AutoRegularInspection.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using AutoRegularInspection.Services;
using System.Linq;
using System.Diagnostics;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //var im = new ImageClass(FileService.GetFileName($"{App.PicturesFolder}", "DSC07952"));
            //try
            //{
            //    using (Image image = Image.Load<Rgba32>($"{App.PicturesFolder}\\DSC07952.jpg"))
            //    {
            //        int width = image.Width / 8;
            //        int height = image.Height / 8;
            //        image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));
            //        image.Save($"{App.PicturesOutFolder}\\DSC07952-out.jpg");
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message.ToString(CultureInfo.InvariantCulture));
            //}

            if (MessageBox.Show("保存后将会覆盖原来的Excel文件，你确定要继续吗？", "保存Excel", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
                var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

                if (SaveExcelService.SaveExcel(_bridgeDeckListDamageSummary.OrderBy(x => x.Component).ThenBy(x => x.Damage).ToList()
                    , _superSpaceListDamageSummary.OrderBy(x => x.Component).ThenBy(x => x.Damage).ToList()
                    , _subSpaceListDamageSummary.OrderBy(x => x.Component).ThenBy(x => x.Damage).ToList(), "外观检查-排序.xlsx") == 1)
                {
                    if(MessageBox.Show("Excel保存成功！文件名为：外观检查-排序.xlsx", "排序完成", MessageBoxButton.YesNoCancel, MessageBoxImage.Information) ==MessageBoxResult.Yes)
                    {
                        if (File.Exists("外观检查-排序.xlsx"))
                        {
                            Process.Start("外观检查-排序.xlsx");
                        }
                        else
                        {
                            MessageBox.Show($"请先进行排序。");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Excel保存失败！");
                }
            }

        }


    }
}
