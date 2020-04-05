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
    public class BridgeDamage : INotifyPropertyChanged
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Idx">索引</param>
        /// <param name="Id">Id</param>
        /// <param name="Title">标题</param>
        //public BridgeDamage(int Idx, int Id,string Title)
        //{
        //    _Idx = Idx;
        //    _Id = Id;
        //    _Title = Title;
        //}



        /// <summary>
        /// 实际为Index（命名为Idx防止和关键字重名）
        /// </summary>
        private int _Idx;
        public int Idx
        {
            get { return _Idx; }
            set
            {
                UpdateProperty(ref _Idx, value);
            }
        }
        /// <summary>
        /// Id相当于枚举值
        /// </summary>
        private int _Id;
        public int Id
        {
            get { return _Id; }
            set
            {
                UpdateProperty(ref _Id, value);
            }
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                UpdateProperty(ref _Title, value);
            }
        }

        private ObservableCollection<BridgeDamage>  _DamageComboBox;
        public ObservableCollection<BridgeDamage> DamageComboBox
        {
            get { return _DamageComboBox; }
            set
            {
                UpdateProperty(ref _DamageComboBox, value);
            }
        }
        //public ObservableCollection<BridgeDamage> DamageComboBox { set; get; }

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
    }
    
}
