using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 病害汇总
    /// </summary>
    public class DamageSummary
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int No { set; get; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Position { set; get; }

        public BridgeDeckEnum TestEnum { set; get; }

        public BindingList<BridgeDeck> TestComboBox { set; get; }

        public int TestValue { set; get; }
        /// <summary>
        /// 构件类型
        /// </summary>
        public string Component { set; get; }

        /// <summary>
        /// 损坏类型
        /// </summary>
        public string Damage { set; get; }

        /// <summary>
        /// 病害描述
        /// </summary>
        public string DamageDescription { set; get; }

        /// <summary>
        /// 病害对应图片描述
        /// </summary>
        public string DamageDescriptionInPicture { set; get; }

        /// <summary>
        /// 病害对应图片编号
        /// </summary>
        public string PictureNo { set; get; }

        //以下为扩展字段（根据其它已知信息可以推算出内容的字段）

        /// <summary>
        /// 病害对应图片编号
        /// </summary>
        public System.Windows.Media.Imaging.BitmapImage PicturePreview { get; set; }

        /// <summary>
        /// 图片数量
        /// </summary>
        public int PictureCounts { set; get; }

        /// <summary>
        /// 首张图片编号
        /// </summary>
        public string FirstPictureNo { set; get; }
        /// <summary>
        /// 首张图片书签索引
        /// </summary>
        public int FirstPictureBookmarkIndex { set; get; }

        /// <summary>
        /// 首张图片书签
        /// </summary>
        public string FirstPictureBookmark { set; get; }
        /// <summary>
        /// 最后一张图片编号
        /// </summary>
        public string LastPictureNo { set; get; }
        /// <summary>
        /// 最后一张图片书签
        /// </summary>
        public string LastPictureBookmark { set; get; }
    }
}
