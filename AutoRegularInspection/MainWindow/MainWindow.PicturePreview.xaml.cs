

using AutoRegularInspection.Models;
using AutoRegularInspection.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace AutoRegularInspection
{
    public partial class MainWindow : Window
    {
        private void PicturePreview_Click(object sender, RoutedEventArgs e)
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
            Image image = null;
            ImageClass imageClass = null;
            string picture1 = string.Empty; string picture2 = string.Empty;


            OpenPicturePerview(_bridgeDeckListDamageSummary, l1);
            OpenPicturePerview(_superSpaceListDamageSummary, l2);
            OpenPicturePerview(_subSpaceListDamageSummary, l3);
            void OpenPicturePerview(ObservableCollection<DamageSummary> listDamageSummary, List<DamageSummary> lst)
            {
                string[] dirs, outdirs;
                if (!File.Exists(@"日志.txt"))
                {
                    _ = File.Create(@"日志.txt");
                }
                Image img = null;
                Bitmap map = null ; Bitmap map2 = null;
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].PictureCounts == 0)
                    {
                        continue;
                    }
                    else if (lst[i].PictureCounts == 1)
                    {
                        //已被重构
                        //imageClass = new ImageClass(Directory.GetFiles(@"Pictures/", $"*{lst[i].PictureNo}*")[0]);

                        try
                        {
                            dirs = Directory.GetFiles($@"{App.PicturesFolder}/", $"*{lst[i].PictureNo}.*");    //结果含有路径
                            outdirs = Directory.GetFiles($@"{App.PicturesOutFolder}/", $"*{lst[i].PictureNo}.*");
                            if(dirs.Length!=0)
                            {
                                imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesFolder}", lst[i].PictureNo));
                                
                            }
                            else
                            {
                                if (outdirs.Length != 0)
                                {
                                    imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesOutFolder}", lst[i].PictureNo));
                                }
                                else
                                {
                                    imageClass = new ImageClass("ErrorPic.jpg");
                                }
                            }
                            
                        }
                        catch (System.Exception ex)
                        {
                            imageClass = new ImageClass("ErrorPic.jpg");

                            var stream = new FileStream(@"日志.txt", FileMode.Append);//fileMode指定是读取还是写入
                            StreamWriter writer = new StreamWriter(stream);
                            writer.WriteLine($"警告：{lst[i].PictureNo}不存在！错误信息：{ex.Message}");
                            writer.Close();
                            stream.Close();
                            
                        }

                        img = imageClass.GetReducedImage(0.2);
                        //img = Image.FromFile($"{Directory.GetFiles(@"PicturesOut/", $"*{lst[i].PictureNo}*")[0]}");
                        map = new Bitmap(img);

                        listDamageSummary[i].PictureHeight = 60;
                        listDamageSummary[i].PicturePreview1 = ConvertBitmap(map);
                    }
                    else if (lst[i].PictureCounts >= 2)
                    {
                        var pictures = lst[i].PictureNo.Split(App.PictureNoSplitSymbol);

                        //imageClass = new ImageClass(Directory.GetFiles(@"Pictures/", $"*{pictures[0]}*")[0]);
                        
                        try
                        {
                            //imageClass = new ImageClass(FileService.GetFileName(@"Pictures", pictures[0]));

                            dirs = Directory.GetFiles($@"{App.PicturesFolder}/", $"*{pictures[0]}.*");    //结果含有路径
                            outdirs = Directory.GetFiles($@"{App.PicturesOutFolder}/", $"*{pictures[0]}.*");
                            if (dirs.Length != 0)
                            {
                                imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesFolder}", pictures[0]));

                            }
                            else
                            {
                                if (outdirs.Length != 0)
                                {
                                    imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesOutFolder}", pictures[0]));
                                }
                                else
                                {
                                    imageClass = new ImageClass("ErrorPic.jpg");
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            imageClass = new ImageClass("ErrorPic.jpg");

                            var stream = new FileStream(@"日志.txt", FileMode.Append);//fileMode指定是读取还是写入
                            StreamWriter writer = new StreamWriter(stream);
                            writer.WriteLine($"警告：{pictures[0]}不存在！错误信息：{ex.Message}");
                            writer.Close();
                            stream.Close();
                        }

                        img = imageClass.GetReducedImage(0.2);

                        //img = Image.FromFile($"{Directory.GetFiles(@"Pictures/", $"*{pictures[0]}*")[0]}");
                        map = new Bitmap(img);

                        listDamageSummary[i].PicturePreview1 = ConvertBitmap(map);

                        //imageClass = new ImageClass(Directory.GetFiles(@"Pictures/", $"*{pictures[1]}*")[0]);
                        try
                        {
                            //imageClass = new ImageClass(FileService.GetFileName(@"Pictures", pictures[1]));

                            dirs = Directory.GetFiles($@"{App.PicturesFolder}/", $"*{pictures[1]}.*");    //结果含有路径
                            outdirs = Directory.GetFiles($@"{App.PicturesOutFolder}/", $"*{pictures[1]}.*");
                            if (dirs.Length != 0)
                            {
                                imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesFolder}", pictures[1]));

                            }
                            else
                            {
                                if (outdirs.Length != 0)
                                {
                                    imageClass = new ImageClass(FileService.GetFileName($"{App.PicturesOutFolder}", pictures[1]));
                                }
                                else
                                {
                                    imageClass = new ImageClass("ErrorPic.jpg");
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            imageClass = new ImageClass("ErrorPic.jpg");

                            var stream = new FileStream(@"日志.txt", FileMode.Append);//fileMode指定是读取还是写入
                            StreamWriter writer = new StreamWriter(stream);
                            writer.WriteLine($"警告：{pictures[1]}不存在！错误信息：{ex.Message}");
                            writer.Close();
                            stream.Close();
                        }
                        img = imageClass.GetReducedImage(0.2);

                        //img = Image.FromFile($"{Directory.GetFiles(@"Pictures/", $"*{pictures[1]}*")[0]}");
                        map2 = new Bitmap(img);
                        listDamageSummary[i].PicturePreview2 = ConvertBitmap(map2);
                        listDamageSummary[i].PictureHeight = 60;
                    }
                    else    //异常、负数等情况
                    {
                        listDamageSummary[i].PicturePreview1 = null;
                        listDamageSummary[i].PicturePreview2 = null;
                    }

                }
            }
        }

        private void ClosePicturePreview_Click(object sender, RoutedEventArgs e)
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
            Image image = null; Bitmap bitmap = null;
   
            ClosePicturePerview(_bridgeDeckListDamageSummary, l1, ref image, ref bitmap);
            ClosePicturePerview(_superSpaceListDamageSummary, l2, ref image, ref bitmap);
            ClosePicturePerview(_subSpaceListDamageSummary, l3, ref image, ref bitmap);
            void ClosePicturePerview(ObservableCollection<DamageSummary> listDamageSummary, List<DamageSummary> lst, ref Image img, ref Bitmap map)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    listDamageSummary[i].PictureHeight = 15;
                    listDamageSummary[i].PicturePreview1 = null;
                    listDamageSummary[i].PicturePreview2 = null;
                }
            }
        }



        private BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }


}
