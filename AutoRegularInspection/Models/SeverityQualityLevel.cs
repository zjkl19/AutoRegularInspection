using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class SeverityQualityLevel : INotifyPropertyChanged
    {
        private int _Idx;
        public int Idx
        {
            get { return _Idx; }
            set
            {
                UpdateProperty(ref _Idx, value);
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

        private string _DisplayTitle;
        public string DisplayTitle
        {
            get { return _DisplayTitle; }
            set
            {
                UpdateProperty(ref _DisplayTitle, value);
            }
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
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
