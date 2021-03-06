﻿

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
            Image image = null; Bitmap bitmap = null;
            ImageClass imageClass = null;
            string picture1 = string.Empty; string picture2 = string.Empty;


            OpenPicturePerview(_bridgeDeckListDamageSummary, l1, ref image, ref bitmap);
            OpenPicturePerview(_superSpaceListDamageSummary, l2, ref image, ref bitmap);
            OpenPicturePerview(_subSpaceListDamageSummary, l3, ref image, ref bitmap);
            void OpenPicturePerview(ObservableCollection<DamageSummary> listDamageSummary, List<DamageSummary> lst, ref Image img, ref Bitmap map)
            {
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
                        
                        imageClass = new ImageClass(FileService.GetFileName(@"Pictures", lst[i].PictureNo));
                        img = imageClass.GetReducedImage(0.2);
                        //img = Image.FromFile($"{Directory.GetFiles(@"PicturesOut/", $"*{lst[i].PictureNo}*")[0]}");
                        map = new Bitmap(img);

                        listDamageSummary[i].PictureHeight = 60;
                        listDamageSummary[i].PicturePreview1 = ConvertBitmap(map);
                    }
                    else if (lst[i].PictureCounts >= 2)
                    {
                        var pictures = lst[i].PictureNo.Split(',');

                        //imageClass = new ImageClass(Directory.GetFiles(@"Pictures/", $"*{pictures[0]}*")[0]);
                        imageClass = new ImageClass(FileService.GetFileName(@"Pictures", pictures[0]));
                        img = imageClass.GetReducedImage(0.2);

                        //img = Image.FromFile($"{Directory.GetFiles(@"Pictures/", $"*{pictures[0]}*")[0]}");
                        map = new Bitmap(img);


                        listDamageSummary[i].PicturePreview1 = ConvertBitmap(map);

                        //imageClass = new ImageClass(Directory.GetFiles(@"Pictures/", $"*{pictures[1]}*")[0]);
                        imageClass = new ImageClass(FileService.GetFileName(@"Pictures", pictures[1]));
                        img = imageClass.GetReducedImage(0.2);

                        //img = Image.FromFile($"{Directory.GetFiles(@"Pictures/", $"*{pictures[1]}*")[0]}");
                        map = new Bitmap(img);
                        listDamageSummary[i].PicturePreview2 = ConvertBitmap(map);
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
