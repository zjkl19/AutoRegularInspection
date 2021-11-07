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
    public class OptionModel : INotifyPropertyChanged
    {
        private string _PictureWidth;
        public string PictureWidth
        {
            get { return _PictureWidth; }
            set
            {
                UpdateProperty(ref _PictureWidth, value);
            }
        }

        private string _PictureHeight;
        public string PictureHeight
        {
            get { return _PictureHeight; }
            set
            {
                UpdateProperty(ref _PictureHeight, value);
            }
        }

        private string _BridgeDeckBookmarkStartNo;
        public string BridgeDeckBookmarkStartNo
        {
            get { return _BridgeDeckBookmarkStartNo; }
            set
            {
                UpdateProperty(ref _BridgeDeckBookmarkStartNo, value);
            }
        }

        private string _SuperSpaceBookmarkStartNo;
        public string SuperSpaceBookmarkStartNo
        {
            get { return _SuperSpaceBookmarkStartNo; }
            set
            {
                UpdateProperty(ref _SuperSpaceBookmarkStartNo, value);
            }
        }

        private string _SubSpaceBookmarkStartNo;
        public string SubSpaceBookmarkStartNo
        {
            get { return _SubSpaceBookmarkStartNo; }
            set
            {
                UpdateProperty(ref _SubSpaceBookmarkStartNo, value);
            }
        }

        private string _BridgeDeckNoWidth;
        public string BridgeDeckNoWidth
        {
            get { return _BridgeDeckNoWidth; }
            set
            {
                UpdateProperty(ref _BridgeDeckNoWidth, value);
            }
        }

        private string _BridgeDeckPositionWidth;
        public string BridgeDeckPositionWidth
        {
            get { return _BridgeDeckPositionWidth; }
            set
            {
                UpdateProperty(ref _BridgeDeckPositionWidth, value);
            }
        }

        //private object _SubPage;
        //public object SubPage
        //{
        //    get { return _SubPage; }
        //    set
        //    {
        //        UpdateProperty(ref _SubPage, value);
        //    }
        //}

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
