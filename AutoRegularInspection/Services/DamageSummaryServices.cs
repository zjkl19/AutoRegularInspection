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
    public static class DamageSummaryServices
    {
        /// <summary>
        /// 初始化病害汇总列表
        /// </summary>
        /// <param name="listDamageSummary"></param>
        /// <param name="firstIndex"></param>
        public static void InitListDamageSummary(List<DamageSummary> listDamageSummary, int firstIndex = 1000000,BridgePart bridgePart=BridgePart.BridgeDeck)
        {
            SetPictureCounts(listDamageSummary);
            SetFirstAndLastPictureBookmark(listDamageSummary, firstIndex);
            SetComboBox(listDamageSummary, bridgePart);

            //for (int i = 0; i < listDamageSummary.Count; i++)
            //{
            //    var img = System.Drawing.Image.FromFile($"PicturesOut/DSC00855.jpg");
            //    var map = new System.Drawing.Bitmap(img);
            //    listDamageSummary[i].PicturePreview= ConvertBitmap(map);

            //}
        }

        private static void SetComboBox(List<DamageSummary> listDamageSummary, BridgePart bridgePart = BridgePart.BridgeDeck)
        {
            for (int i = 0; i < listDamageSummary.Count; i++)
            {
                //listDamageSummary[i].ComponentComboBox = new BindingList<BridgeDeck>
                //{
                //    new BridgeDeck{  Id=1,Title="桥面铺装"}
                //    ,new BridgeDeck{  Id=2,Title="其它"}
                //};

                //TODO：写单元测试
                //创建映射
                IEnumerable<BridgeDeck> componentFound = null;

                if (bridgePart == BridgePart.BridgeDeck)
                {
                    componentFound = GlobalData.ComponentComboBox.Where(x => x.Title == listDamageSummary[i].Component);
                }
                else if (bridgePart == BridgePart.SuperSpace)
                {
                    componentFound = GlobalData.SuperSpaceComponentComboBox.Where(x => x.Title == listDamageSummary[i].Component);
                }
                else
                {
                    componentFound = GlobalData.ComponentComboBox.Where(x => x.Title == listDamageSummary[i].Component);
                }

                if (componentFound.Any())
                {
                    listDamageSummary[i].ComponentValue = componentFound.FirstOrDefault().Idx;

                    var damageFound = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == listDamageSummary[i].Damage);

                    //if (damageFound.Any())
                    //{
                    //    listDamageSummary[i].TestComboBox1 = damageFound.FirstOrDefault().DamageComboBox;

                    //    listDamageSummary[i].TestValue1 = damageFound.FirstOrDefault().DamageComboBox.FirstOrDefault().Idx;
                    //}

                    if (damageFound.Any())
                    {
                        //listDamageSummary[i].TestComboBox1 = new BindingList<BridgeDeck>(damageFound.ToList());
                        listDamageSummary[i].DamageComboBox = componentFound.FirstOrDefault().DamageComboBox;

                        listDamageSummary[i].DamageValue = damageFound.FirstOrDefault().Idx;
                    }
                    else
                    {
                        listDamageSummary[i].DamageComboBox = componentFound.FirstOrDefault().DamageComboBox;

                        listDamageSummary[i].DamageValue = componentFound.FirstOrDefault().DamageComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;
                    }

                }
                else
                {

                    listDamageSummary[i].DamageComboBox = GlobalData.ComponentComboBox.Where(x => x.Title == "其它").FirstOrDefault().DamageComboBox;
                    listDamageSummary[i].ComponentValue = GlobalData.ComponentComboBox.Where(x => x.Title == "其它").FirstOrDefault().Idx;
                }

            }
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

        public static System.Windows.Media.Imaging.BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
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