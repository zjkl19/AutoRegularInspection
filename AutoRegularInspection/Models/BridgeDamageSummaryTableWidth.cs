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
        private double _No;
        public double No
        {
            get { return _No; }
            set => UpdateProperty(ref _No, value);
        }

        private double _Position;
        public double Position
        {
            get { return _Position; }
            set => UpdateProperty(ref _Position, value);
        }
        private double _Component;
        public double Component
        {
            get { return _Component; }
            set => UpdateProperty(ref _Component, value);
        }
        private double _Damage;
        public double Damage
        {
            get { return _Damage; }
            set => UpdateProperty(ref _Damage, value);
        }
        private double _DamagePosition;
        public double DamagePosition
        {
            get { return _DamagePosition; }
            set => UpdateProperty(ref _DamagePosition, value);
        }

        private double _DamageDescription;
        public double DamageDescription
        {
            get { return _DamageDescription; }
            set => UpdateProperty(ref _DamageDescription, value);
        }
        private double _PictureNo;
        public double PictureNo
        {
            get { return _PictureNo; }
            set => UpdateProperty(ref _PictureNo, value);
        }


        private double _Comment;

        public double Comment
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
