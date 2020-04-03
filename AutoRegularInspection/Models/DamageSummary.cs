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

        private BindingList<BridgeDeck> _DamageComboBox;

        public BindingList<BridgeDeck> DamageComboBox
        {
            get { return _DamageComboBox; }
            set
            {
                _DamageComboBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DamageComboBox)));
            }
        }
        private int _DamageValue { set; get; }
        public int DamageValue
        {
            get { return _DamageValue; }
            set
            {
                _DamageValue = value;

                OnPropertyChanged(nameof(DamageValue));
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
        public string Component { set; get; } = string.Empty;

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
        /// <summary>
        /// 直接获取部位名
        /// </summary>
        /// <returns></returns>
        public string GetComponentName()
        {
            if (GlobalData.ComponentComboBox[ComponentValue].Title != "其它")
            {
                return(GlobalData.ComponentComboBox[ComponentValue].Title);
            }
            else    //TODO:考虑"其它"输入为空的情况
            {
                return Component;
            }
        }
        /// <summary>
        /// 直接获取病害名
        /// </summary>
        /// <returns></returns>
        public string GetDamageName()
        {
            if (GlobalData.ComponentComboBox[ComponentValue].DamageComboBox[DamageValue].Title != "其它")
            {
                return (GlobalData.ComponentComboBox[ComponentValue].DamageComboBox[DamageValue].Title);
            }
            else    //TODO:考虑"其它"输入为空的情况
            {
                return Damage;
            }
        }
    }
}
