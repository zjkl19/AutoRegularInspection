using AutoRegularInspection.IRepository;
using AutoRegularInspection.Models;
using AutoRegularInspection.Repository;
using AutoRegularInspection.Views;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace AutoRegularInspection.ViewModels
{
    public class OptionViewModel : INotifyPropertyChanged
    {
        private readonly ILogger _log;
        public ObservableCollection<Option> Options { get; set; }

        public OptionConfiguration OptionConfiguration { get; set; }

        private Option _selectedOption;
        public Option SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                NotifyPropertyChanged(nameof(SelectedOption));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveCommand { get; private set; }

        public OptionViewModel()
        {
            //Nlog
            LoggingConfiguration config = new LoggingConfiguration();
            // Targets where to log to: File and Console
            FileTarget logfile = new FileTarget("logfile") { FileName = @"Log\LogFile.txt" };
            ConsoleTarget logconsole = new ConsoleTarget("logconsole");
            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);
            // Apply config           
            LogManager.Configuration = config;
            _log = LogManager.GetCurrentClassLogger();

            // Here we can add the options with their corresponding user controls and children
            Options = new ObservableCollection<Option>
            {
                new Option { Name = "图片", UserControl = new PictureOptionPage(), Children = new List<Option>
                {
                    new Option { Name = "常规", UserControl = new PictureOptionPage()},

                }},
                new Option { Name = "报告", UserControl = new GeneralOptionPage(), Children = new List<Option>
                {
                    new Option { Name = "常规", UserControl = new GeneralOptionPage()},
                    new Option { Name = "书签", UserControl = new BookmarkOptionPage()},
                    new Option { Name = "汇总表格", UserControl = new SummaryTableOptionPage()},
                }},
                //new Option { Name = "ParentOption2", UserControl = new Option2UserControl(), Children = new List<Option>
                //{
                //    new Option { Name = "ChildOption3", UserControl = new Option2UserControl() },
                //    new Option { Name = "ChildOption4", UserControl = new Option1UserControl() },
                //}},
                // add more options as needed
            };
            //反序列化XML配置文件
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(OptionConfiguration));
            StreamReader streamReader = new StreamReader($"{App.ConfigurationFolder}\\{App.ConfigFileName}");
            StreamReader reader = streamReader;    //TODO：找不到文件的判断
            var deserializedConfig = (OptionConfiguration)serializer.Deserialize(reader);    //DataContext

            //Options[0].UserControl.DataContext = deserializedConfig;
            //Options[0].Children[0].UserControl.DataContext = deserializedConfig;
            //Options[1].UserControl.DataContext = deserializedConfig;
            //Options[1].Children[0].UserControl.DataContext = deserializedConfig;
            //设置DataContext
            for (int i = 0; i < Options.Count; i++)
            {
                Options[i].UserControl.DataContext = deserializedConfig;
                for (int j = 0; j < Options[i].Children.Count; j++)
                {
                    Options[i].Children[j].UserControl.DataContext = deserializedConfig;
                }
            }

            SaveCommand = new RelayCommand(Save);
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Save(object parameter)
        {
            try
            {
                var configuration = Options[0].UserControl.DataContext;    //获得DataContext
                                                                           //XmlSerializer serializer = new XmlSerializer(typeof(OptionConfiguration));
                                                                           //using (TextWriter writer = new StreamWriter($"{App.ConfigurationFolder}\\{App.ConfigFileName}"))
                                                                           //{
                                                                           //    serializer.Serialize(writer, configuration);
                                                                           //}
                IFileWriter fileWriter = new FileWriter();
                IXmlSerializer<OptionConfiguration> serializer = new LocalXmlSerializer<OptionConfiguration>();
                SaveFile(configuration, fileWriter, serializer);

                _ = MessageBox.Show("保存设置成功！");
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error occurred in Save method");
                throw; // rethrow the exception if you want it to be handled elsewhere

            }

        }

        public static void SaveFile(object configuration, IFileWriter fileWriter, IXmlSerializer<OptionConfiguration> serializer)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Configuration cannot be null");
            }
            using (var textWriter = fileWriter.Create($"{App.ConfigurationFolder}\\{App.ConfigFileName}"))
            {
                if (textWriter == null)
                {
                    throw new Exception("Failed to create TextWriter.");
                }
                serializer.Serialize(textWriter, (OptionConfiguration)configuration);
            }
            //using (var textWriter = fileWriter.Create($"{App.ConfigurationFolder}\\{App.ConfigFileName}"))
            //{
            //    serializer.Serialize(textWriter, (OptionConfiguration)configuration);
            //}  
        }
    }
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}
