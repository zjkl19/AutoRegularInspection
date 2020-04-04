using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRegularInspection.Models
{
    public class ProgressBarModel : INotifyPropertyChanged
    {
        private int _ProgressValue = 0;

        public int ProgressValue
        {
            get { return _ProgressValue; }
            set
            {
                _ProgressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        private string _Content = string.Empty;
        public string Content
        {
            get { return _Content; }
            set
            {
                _Content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
