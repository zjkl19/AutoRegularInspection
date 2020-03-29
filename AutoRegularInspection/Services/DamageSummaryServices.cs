using AutoRegularInspection.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace AutoRegularInspection.Services
{
    /// <summary>
    /// 对List<DamageSummary>数据进行初始化、预处理
    /// </summary>
    public class DamageSummaryServices
    {
        /// <summary>
        /// 初始化病害汇总列表
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <param name="firstIndex"></param>
        public static void InitListDamageSummary(List<DamageSummary> listDamageSummary, int firstIndex = 1000000)
        {
            SetPictureCounts(listDamageSummary);
            SetFirstAndLastPictureBookmark(listDamageSummary, firstIndex);

            for (int i = 0; i < listDamageSummary.Count; i++)
            {

                foreach (int v in Enum.GetValues(typeof(BridgeDeckEnum)))
                {
                    if (EnumHelper.GetEnumDesc((BridgeDeckEnum)v).ToString() == listDamageSummary[i].Component)
                    {
                        listDamageSummary[i].TestEnum = (BridgeDeckEnum)v;
                        break;
                    }
                    else
                    {
                        listDamageSummary[i].TestEnum = BridgeDeckEnum.Others;
                    }
                }

                //if (listDamageSummary[i].Component=="伸缩缝")
                //{
                //    listDamageSummary[i].TestEnum = BridgePart.BridgeDeck;
                //}
                //else
                //{
                //    listDamageSummary[i].TestEnum = BridgePart.SubSpace;
                //}
            }

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                //listDamageSummary[i].ComponentComboBox = new BindingList<BridgeDeck>
                //{
                //    new BridgeDeck{  Id=1,Title="桥面铺装"}
                //    ,new BridgeDeck{  Id=2,Title="其它"}
                //};

                if(GlobalData.ComponentComboBox.Where(x=>x.Title== listDamageSummary[i].Component).Any())
                {
                    listDamageSummary[i].ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == listDamageSummary[i].Component).FirstOrDefault().Id-1 ;
                }
                //if (i == 0)
                //{

                //    listDamageSummary[i].ComponentValue = 0;
                //}
                //else
                //{

                //    listDamageSummary[i].ComponentValue = 1;
                //}

                if (i == 0)
                {

                    listDamageSummary[i].TestComboBox1 = new BindingList<BridgeDeck>
                {
                    new BridgeDeck{  Id=1,Title="阻塞"}
                    ,new BridgeDeck{  Id=2,Title="碎边"}
                };
                    listDamageSummary[i].TestValue1 = 0;
                }
                else
                {

                    listDamageSummary[i].TestComboBox1 = new BindingList<BridgeDeck>
                {
                    new BridgeDeck{  Id=1,Title="阻塞2"}
                    ,new BridgeDeck{  Id=2,Title="碎边2"}
                };
                    listDamageSummary[i].TestValue1 = 1;
                }


            }


            //for (int i = 0; i < listDamageSummary.Count; i++)
            //{
            //    var img = System.Drawing.Image.FromFile($"PicturesOut/DSC00855.jpg");
            //    var map = new System.Drawing.Bitmap(img);
            //    listDamageSummary[i].PicturePreview= ConvertBitmap(map);

            //}
        }

        private static void SetPictureCounts(List<DamageSummary> listDamageSummary)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                listDamageSummary[i].PictureCounts = listDamageSummary[i].PictureNo.Split(',').Count();
            }
        }
        /// <summary>
        /// 要考虑PirctureCounts为0的情况
        /// </summary>
        /// <param name="listDamageSummary"></param>
        private static void SetFirstAndLastPictureBookmark(List<DamageSummary> listDamageSummary, int firstIndex = 1000000)
        {

            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                if (i == 0)
                {
                    listDamageSummary[i].FirstPictureBookmarkIndex = firstIndex;
                }
                else
                {
                    listDamageSummary[i].FirstPictureBookmarkIndex = listDamageSummary[i - 1].FirstPictureBookmarkIndex + listDamageSummary[i - 1].PictureCounts;
                }
                listDamageSummary[i].FirstPictureBookmark = $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex}";
                listDamageSummary[i].LastPictureBookmark = $"_Ref{listDamageSummary[i].FirstPictureBookmarkIndex + listDamageSummary[i].PictureCounts - 1}";
            }
        }

        public System.Windows.Media.Imaging.BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            var image = new System.Windows.Media.Imaging.BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}