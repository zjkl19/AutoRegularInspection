using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 病害汇总
    /// </summary>
    public class DamageSummary : INotifyPropertyChanged
    {
        private BridgePart _BridgePartEnum;

        public BridgePart BridgePartEnum
        {
            get { return _BridgePartEnum; }
            set
            {
                UpdateProperty(ref _BridgePartEnum, value);
            }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int No { set; get; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Position { set; get; }


        private ObservableCollection<BridgeDamage> _DamageComboBox;

        public ObservableCollection<BridgeDamage> DamageComboBox
        {
            get { return _DamageComboBox; }
            set
            {
                UpdateProperty(ref _DamageComboBox, value);
            }
        }
        private int _DamageValue;
        public int DamageValue
        {
            get { return _DamageValue; }
            set
            {
                UpdateProperty(ref _DamageValue, value);
            }
        }
        /// <summary>
        /// 构件类型对应的Combobox
        /// </summary>
        public ObservableCollection<BridgeDamage> ComponentComboBox { set; get; }

        private int _ComponentValue;
        /// <summary>
        /// 构件类型对应的Combobox对应的Id值
        /// </summary>
        public int ComponentValue
        {
            get { return _ComponentValue; }
            set
            {
                UpdateProperty(ref _ComponentValue, value);
            }
        }

        /// <summary>
        /// 构件类型
        /// </summary>
        public string Component { set; get; } = string.Empty;

        /// <summary>
        /// 损坏类型
        /// </summary>
        public string Damage { set; get; } = string.Empty;


        private string _DamageDescription;
        /// <summary>
        /// 病害描述
        /// </summary>
        public string DamageDescription
        {
            get { return _DamageDescription; }
            set
            {
                UpdateProperty(ref _DamageDescription, value);
            }
        }


        private string _DamageDescriptionInPicture;
        /// <summary>
        /// 病害对应图片描述
        /// </summary>
        public string DamageDescriptionInPicture
        {
            get { return _DamageDescriptionInPicture; }
            set
            {

                UpdateProperty(ref _DamageDescriptionInPicture, value);
            }
        }
        /// <summary>
        /// 病害对应图片编号
        /// </summary>
        public string PictureNo { set; get; } = string.Empty;

        private string _Comment;
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment

        {
            get { return _Comment; }
            set
            {
                UpdateProperty(ref _Comment, value);
            }
        }

        private int _Unit1Value;
        public int Unit1Value
        {
            get { return _Unit1Value; }
            set
            {
                UpdateProperty(ref _Unit1Value, value);
            }
        }

        private bool _UseCustomedUnit;
        public bool UseCustomedUnit
        {
            get { return _UseCustomedUnit; }
            set
            {
                UpdateProperty(ref _UseCustomedUnit, value);
            }
        }
        public string Unit1 { set; get; } = string.Empty;

        private int _Unit1Counts;
        public int Unit1Counts
        {
            get { return _Unit1Counts; }
            set
            {
                UpdateProperty(ref _Unit1Counts, value);
            }
        }

        private int _Unit2Value;
        public int Unit2Value
        {
            get { return _Unit2Value; }
            set
            {
                UpdateProperty(ref _Unit2Value, value);
            }
        }

        public string Unit2 { set; get; } = string.Empty;

        private decimal _Unit2Counts;
        public decimal Unit2Counts
        {
            get { return _Unit2Counts; }
            set
            {
                UpdateProperty(ref _Unit2Counts, value);
            }
        }

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

        private void UpdateProperty<T>(ref T properValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(properValue, newValue))
            {
                return;
            }
            properValue = newValue;

            OnPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 直接获取部位名
        /// </summary>
        /// <returns></returns>
        public string GetComponentName(BridgePart bridgePart=BridgePart.BridgeDeck)
        {
            ObservableCollection<BridgeDamage> componentBox = GlobalData.ComponentComboBox;

            if (bridgePart== BridgePart.BridgeDeck)
            {
                componentBox = GlobalData.ComponentComboBox;
            }
            else if(bridgePart == BridgePart.SuperSpace)
            {
                componentBox = GlobalData.SuperSpaceComponentComboBox;
            }
            else
            {
                componentBox = GlobalData.SubSpaceComponentComboBox;
            }

            if (componentBox[ComponentValue].Title != "其它")
            {
                return (componentBox[ComponentValue].Title);
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
        public string GetDamageName(BridgePart bridgePart = BridgePart.BridgeDeck)
        {
            ObservableCollection<BridgeDamage> componentBox = GlobalData.ComponentComboBox;

            if (bridgePart == BridgePart.BridgeDeck)
            {
                componentBox = GlobalData.ComponentComboBox;
            }
            else if (bridgePart == BridgePart.SuperSpace)
            {
                componentBox = GlobalData.SuperSpaceComponentComboBox;
            }
            else
            {
                componentBox = GlobalData.SubSpaceComponentComboBox;
            }

            if(DamageValue<0)
            {
                return Damage;
            }

            if (componentBox[ComponentValue].DamageComboBox[DamageValue].Title != "其它")
            {
                return (componentBox[ComponentValue].DamageComboBox[DamageValue].Title);
            }
            else    //TODO:考虑"其它"输入为空的情况
            {
                return Damage;
            }
        }

        public string GetUnit1()
        {
            ObservableCollection<StatisticsUnit> unitBox = GlobalData.Unit1ComboBox;
            return (unitBox[Unit1Value].Title);
        }

        public string GetUnit2()
        {
            ObservableCollection<StatisticsUnit> unitBox = GlobalData.Unit2ComboBox;
            return (unitBox[Unit2Value].Title);
        }

    }
}
