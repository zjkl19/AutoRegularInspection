

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
            Image img = null; Bitmap map = null;
            string picture1 = string.Empty; string picture2 = string.Empty;

            //listDamageSummary[i].PictureNo.Split(',').Length;


            //var p = listDamageSummary[i].PictureNo.Split(',');
            //for (int j = 0; j < p.Length; j++)
            //{
            //    builder.MoveTo(pictureTable.Rows[2 * (int)(curr / 2)].Cells[(curr) % 2].FirstParagraph);

            //    var dirs = Directory.GetFiles(@"Pictures/", $"*{p[j]}*");    //结果含有路径

            //    //TODO：检测文件是否重复，若重复不需要再压缩（MD5校验）
            //    //(暂时用文件名校验)
            //    if (!File.Exists($"PicturesOut/{Path.GetFileName(dirs[0])}"))
            //    {
            //        ImageServices.CompressImage($"{dirs[0]}", $"PicturesOut/{Path.GetFileName(dirs[0])}", CompressImageFlag);    //只取查找到的第1个文件，TODO：UI提示       
            //    }

            for (int i = 0; i < l1.Count; i++)
            {
                if (l1[i].PictureCounts == 0)
                {
                    continue;
                }
                else if (l1[i].PictureCounts == 1)
                {
                    img = Image.FromFile($"{Directory.GetFiles(@"PicturesOut/", $"*{l1[i].PictureNo}*")[0]}");
                    map = new Bitmap(img);

                    _bridgeDeckListDamageSummary[i].PictureHeight = 100;
                    _bridgeDeckListDamageSummary[i].PicturePreview1 = ConvertBitmap(map);
                }
                else if (l1[i].PictureCounts >=2)
                {
                    var pictures = l1[i].PictureNo.Split(',');

                    img = Image.FromFile($"{Directory.GetFiles(@"PicturesOut/", $"*{pictures[0]}*")[0]}");
                    map = new Bitmap(img);

                    
                    _bridgeDeckListDamageSummary[i].PicturePreview1 = ConvertBitmap(map);

                    img = Image.FromFile($"{Directory.GetFiles(@"PicturesOut/", $"*{pictures[1]}*")[0]}");
                    map = new Bitmap(img);
                    _bridgeDeckListDamageSummary[i].PicturePreview2 = ConvertBitmap(map);
                    _bridgeDeckListDamageSummary[i].PictureHeight = 100;
                }
                else    //异常、负数等情况
                {
                    _bridgeDeckListDamageSummary[i].PicturePreview1 = null;
                    _bridgeDeckListDamageSummary[i].PicturePreview2 = null;
                }

                //
                //_bridgeDeckListDamageSummary[i].PicturePreview =  null;

            }
            //for (int i = 0; i < listDamageSummary.Count; i++)
            //{
            //    var img = System.Drawing.Image.FromFile($"PicturesOut/DSC00855.jpg");
            //    var map = new System.Drawing.Bitmap(img);
            //    listDamageSummary[i].PicturePreview= ConvertBitmap(map);

            //}
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
