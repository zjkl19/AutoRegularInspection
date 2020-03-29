﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class BridgeDeck : INotifyPropertyChanged
    {
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
    }
    
}
