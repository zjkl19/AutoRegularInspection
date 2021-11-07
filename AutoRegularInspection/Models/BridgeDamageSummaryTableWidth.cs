using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AutoRegularInspection.Models
{
    /// <summary>
    /// 仅支持整数
    /// </summary>
    public class BridgeDamageSummaryTableWidth
    {
        private int _No;
        public int No
        {
            get { return _No; }
            set => UpdateProperty(ref _No, value);
        }

        private int _Position;
        public int Position
        {
            get { return _Position; }
            set => UpdateProperty(ref _Position, value);
        }
        private int _Component;
        public int Component
        {
            get { return _Component; }
            set => UpdateProperty(ref _Component, value);
        }
        private int _Damage;
        public int Damage
        {
            get { return _Damage; }
            set => UpdateProperty(ref _Damage, value);
        }
        private int _DamageDescription;
        public int DamageDescription
        {
            get { return _DamageDescription; }
            set => UpdateProperty(ref _DamageDescription, value);
        }
        private int _PictureNo;
        public int PictureNo
        {
            get { return _PictureNo; }
            set => UpdateProperty(ref _PictureNo, value);
        }


        private int _Comment;

        public int Comment
        {
            get { return _Comment; }
            set => UpdateProperty(ref _Comment, value);
        }

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
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
