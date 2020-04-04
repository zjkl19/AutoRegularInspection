﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 实际为Index（命名为Idx防止和关键字重名）
        /// </summary>
        private int _Idx;
        public int Idx
        {
            get { return _Idx; }
            set
            {
                _Idx = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Idx)));
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
                _Id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Id)));
            }
        }
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }

        private BindingList<BridgeDamage>  _DamageComboBox;
        public BindingList<BridgeDamage> DamageComboBox
        {
            get { return _DamageComboBox; }
            set
            {
                _DamageComboBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DamageComboBox)));
            }
        }
        //public BindingList<BridgeDamage> DamageComboBox { set; get; }
    }
    
}