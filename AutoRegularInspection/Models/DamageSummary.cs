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
    public class DamageSummary : INotifyPropertyChanged
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



        private BindingList<BridgeDeck> _SubComponentComboBox;

        public BindingList<BridgeDeck> SubComponentComboBox
        {
            get { return _SubComponentComboBox; }
            set
            {
                _SubComponentComboBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SubComponentComboBox)));
            }
        }
        private int _SubComponentValue { set; get; }
        public int SubComponentValue
        {
            get { return _SubComponentValue; }
            set
            {
                _SubComponentValue = value;

                OnPropertyChanged(nameof(SubComponentValue));
            }
        }
        /// <summary>
        /// 构件类型对应的Combobox
        /// </summary>
        public BindingList<BridgeDeck> ComponentComboBox { set; get; }

        private int _ComponentValue;
        /// <summary>
        /// 构件类型对应的Combobox对应的Id值
        /// </summary>
        public int ComponentValue
        {
            get { return _ComponentValue; }
            set
            {
                _ComponentValue = value;

                OnPropertyChanged(nameof(ComponentValue));
            }
        }

        /// <summary>
        /// 构件类型
        /// </summary>
        public string Component { set; get; }

        /// <summary>
        /// 损坏类型
        /// </summary>
        public string Damage { set; get; }


        private string damageDescription;
        /// <summary>
        /// 病害描述
        /// </summary>
        public string DamageDescription

        {
            get { return damageDescription; }
            set
            {
                damageDescription = value;

                OnPropertyChanged(nameof(DamageDescription));
            }
        }


        private string _DamageDescriptionInPicture { set; get; }
        /// <summary>
        /// 病害对应图片描述
        /// </summary>
        public string DamageDescriptionInPicture
        {
            get { return _DamageDescriptionInPicture; }
            set
            {
                _DamageDescriptionInPicture = value;

                OnPropertyChanged(nameof(DamageDescriptionInPicture));
            }
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
