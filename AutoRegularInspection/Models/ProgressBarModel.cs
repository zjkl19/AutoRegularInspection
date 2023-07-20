using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoRegularInspection.Models
{
    public class ProgressBarModel : INotifyPropertyChanged
    {
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        public ICommand CancelCommand => new CommandHandler(() => CancellationTokenSource.Cancel(), true);
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

        // 定义一个事件用于进度更改
        public event Action<string, int> ProgressChanged;

        // 用于触发事件的辅助方法
        public void ReportProgress(string content, int value)
        {
            ProgressValue = value;
            Content = content;
            ProgressChanged?.Invoke(content, value);
        }
    }

    public class CommandHandler : ICommand
    {
        private Action _action;
        private bool _canExecute;

        public CommandHandler(Action action, bool canExecute)
        {
            _action = action;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
