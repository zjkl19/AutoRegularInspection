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
        private void ValidatePicture_Click(object sender, RoutedEventArgs e)
        {
            var _bridgeDeckListDamageSummary = BridgeDeckGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _superSpaceListDamageSummary = SuperSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;
            var _subSpaceListDamageSummary = SubSpaceGrid.ItemsSource as ObservableCollection<DamageSummary>;

            string storeFile = "无效照片列表.txt";
            string[] dirs;
            FileStream stream;

            List<DamageSummary> l1 = _bridgeDeckListDamageSummary.ToList();
            List<DamageSummary> l2 = _superSpaceListDamageSummary.ToList();
            List<DamageSummary> l3 = _subSpaceListDamageSummary.ToList();

            DamageSummaryServices.InitListDamageSummary1(l1);
            DamageSummaryServices.InitListDamageSummary1(l2, 2_000_000);
            DamageSummaryServices.InitListDamageSummary1(l3, 3_000_000);

            ValidPicture(BridgePart.BridgeDeck, _bridgeDeckListDamageSummary, l1);
            ValidPicture(BridgePart.SuperSpace, _superSpaceListDamageSummary, l2);
            ValidPicture(BridgePart.SubSpace, _subSpaceListDamageSummary, l3);

            MessageBox.Show("照片验证完成！结果详见“无效照片列表.txt”");

            void ValidPicture(BridgePart bridgePart, ObservableCollection<DamageSummary> listDamageSummary, List<DamageSummary> lst)
            {
                

                if (!File.Exists(storeFile))
                {
                    _ = File.Create(storeFile);

                }
                stream = new FileStream(storeFile, FileMode.Append);
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine($"当前时间：{DateTime.Now}");
                writer.Close();
                stream.Close();

                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].PictureCounts == 0)    //没有照片，不需要验证
                    {
                        continue;
                    }
                    else if (lst[i].PictureCounts == 1)
                    {
                        dirs = Directory.GetFiles($@"{"Pictures"}/", $"*{lst[i].PictureNo}.*");    //结果含有路径
                        if (dirs.Length==0)
                        {
                            stream = new FileStream(storeFile, FileMode.Append);//fileMode指定是读取还是写入
                            writer = new StreamWriter(stream);
                            writer.WriteLine($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component},{lst[i].Damage}照片{lst[i].PictureNo}不存在");
                            writer.Close();
                            stream.Close();
                        }

                    }
                    else if (lst[i].PictureCounts >= 2)
                    {
                        var pictures = lst[i].PictureNo.Split(App.PictureNoSplitSymbol);

                        for (int j = 0; j < pictures.Length; j++)
                        {
                            dirs = Directory.GetFiles($@"{"Pictures"}/", $"*{pictures[j]}.*");    //结果含有路径
                            if (dirs.Length == 0)
                            {
                                stream = new FileStream(storeFile, FileMode.Append);//fileMode指定是读取还是写入
                                 writer = new StreamWriter(stream);
                                writer.WriteLine($"{EnumHelper.GetEnumDesc(bridgePart)},{lst[i].Component}照片{pictures[j]}不存在");
                                writer.Close();
                                stream.Close();
                            }
                        }                     

                    }
                    else    //异常、负数等情况
                    {
                        stream = new FileStream(storeFile, FileMode.Append);//fileMode指定是读取还是写入
                        writer = new StreamWriter(stream);
                        writer.WriteLine($"未知情况：不属于没有照片，纸片只有1张或多张的情况");
                        writer.Close();
                        stream.Close();

                    }

                }
            }

        }
    }
}
