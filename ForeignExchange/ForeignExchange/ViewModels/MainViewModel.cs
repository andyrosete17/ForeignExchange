﻿
namespace ForeignExchange.ViewModels
{
    using ForeignExchange.Models;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    public class MainViewModel
    {
        #region Properties
        public string Amount { get; set; }
        public ObservableCollection<Rate> Rates { get; set; }
        public Rate SourceRate { get; set; }
        public Rate TargetRate { get; set; }
        public bool IsRunning { get; set; }
        public bool IsEnable { get; set; }
        public string Result { get; set; }

        #endregion Properties
        public MainViewModel()
        {

        }

        #region Commands
        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }
        #endregion Commands
        #region PrivateMethods
        private void Convert()
        {
            
        }
        #endregion PrivateMethods

    }
}
