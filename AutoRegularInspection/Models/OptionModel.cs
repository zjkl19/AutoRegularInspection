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
    public class OptionModel
    {
        private int _Option;
        public int Option
        {
            get { return _Option; }
            set
            {
                UpdateProperty(ref _Option, value);
            }
        }

        private bool _Option1;
        public bool Option1
        {
            get { return _Option1; }
            set
            {
                UpdateProperty(ref _Option1, value);
            }
        }

        private bool _Option2;
        public bool Option2
        {
            get { return _Option2; }
            set
            {
                UpdateProperty(ref _Option2, value);
            }
        }

        private object _SubPage;
        public object SubPage
        {
            get { return _SubPage; }
            set
            {
                UpdateProperty(ref _SubPage, value);
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
