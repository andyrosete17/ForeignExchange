
namespace ForeignExchange.ViewModels
{
    using ForeignExchange.Helpers;
    #region Using
    using ForeignExchange.Models;
    using GalaSoft.MvvmLight.Command;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Net.Http;
    using System.Windows.Input;
    using Xamarin.Forms;
    #endregion Using
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events
        #region Attributes
        bool _isRunning;
        string _result;
        bool _isEnable;
        Rate _sourceRate;
        Rate _targetRate;
        ObservableCollection<Rate> _rates;
        #endregion Attributes

        #region Properties
        public string Amount { get; set; }
        public ObservableCollection<Rate> Rates
        {
            get
            {
                return _rates;
            }
            set
            {
                if (_rates != value)
                {
                    _rates = value;
                    PropertyChanged?.Invoke
                        (
                            this,
                            new PropertyChangedEventArgs(nameof(Rates))
                         );
                }
            }
        }
        public Rate SourceRate
        {
            get
            {
                return _sourceRate;
            }
            set
            {
                if (_sourceRate != value)
                {
                    _sourceRate = value;
                    PropertyChanged?.Invoke
                        (
                            this,
                            new PropertyChangedEventArgs(nameof(SourceRate))
                        );
                }
            }
        }
        public Rate TargetRate
        {
            get
            {
                return _targetRate;
            }
            set
            {
                if (_targetRate != value)
                {
                    _targetRate = value;
                    PropertyChanged?.Invoke
                        (
                            this,
                            new PropertyChangedEventArgs(nameof(TargetRate))
                        );
                }
            }
        }
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    PropertyChanged?.Invoke
                        (
                        this,
                        new PropertyChangedEventArgs(nameof(IsRunning))
                        );
                }
            }
        }
        public bool IsEnable
        {
            get
            {
                return _isEnable;
            }
            set
            {
                if (_isEnable != value)
                {
                    _isEnable = value;
                    PropertyChanged?.Invoke
                        (
                            this,
                            new PropertyChangedEventArgs(nameof(IsEnable))
                        );
                }
            }
        }
        public string Result
        {
            get
            {
                return _result;
            }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    PropertyChanged?.Invoke
                        (
                        this,
                        new PropertyChangedEventArgs(nameof(Result))
                        );
                }
            }
        }
        #endregion Properties
        #region Constructors
        public MainViewModel()
        {
            LoadRates();
        }
        #endregion Constructors

        #region Commands
        public ICommand ConvertCommand
        {
            get
            {
                return new RelayCommand(Convert);
            }
        }
        public ICommand SwitchCommand
        {
            get
            {
                return new RelayCommand(Switch);
            }
        }

        #endregion Commands
        #region PrivateMethods
        async void Convert()
        {
            if (string.IsNullOrWhiteSpace(Amount))
            {
                await Application.Current.MainPage.DisplayAlert
                    (
                        Languages.Error,
                        Languages.AmountValidation,
                        Languages.Accept
                        );
                return;
            }
            decimal amount = 0;
            if (!decimal.TryParse(Amount, out amount))
            {
                await Application.Current.MainPage.DisplayAlert
                                    (
                                        Languages.Error,
                                        Languages.AmountNumericValidation,
                                        Languages.Accept
                                        );
                return;
            }
            if (SourceRate == null)
            {
                await Application.Current.MainPage.DisplayAlert
                                   (
                                        Languages.Error,
                                        Languages.SourceRateValidation,
                                        Languages.Accept
                                       );
                return;
            }

            if (TargetRate == null)
            {
                await Application.Current.MainPage.DisplayAlert
                                   (
                                        Languages.Error,
                                        Languages.TargetRateValidation,
                                        Languages.Accept
                                       );
                return;
            }

            var amountConverted = amount /
                                 (decimal)SourceRate.TaxRate *
                                 (decimal)TargetRate.TaxRate;
            Result = string.Format(
                    "{0} {1:C2} = {2} {3:C2}",
                    SourceRate.Code,
                    amount,
                    TargetRate.Code,
                    amountConverted);
        }

        async void LoadRates()
        {
            IsRunning = true;
            Result =Languages.Loading;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://apiexchangerates.azurewebsites.net");
                var controller = "/api/Rates";
                var response = await client.GetAsync(controller);
                var result = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    IsRunning = false;
                    Result = result;
                }
                var rates = JsonConvert.DeserializeObject<List<Rate>>(result);
                Rates = new ObservableCollection<Rate>(rates);
                IsRunning = false;
                IsEnable = true;
                Result = Languages.Ready;
            }
            catch (Exception ex)
            {
                IsRunning = true;
                Result = ex.Message;
            }

        }

        void Switch()
        {
            var aux = SourceRate;
            SourceRate = TargetRate;
            TargetRate = aux;
            Convert();
        }
        #endregion PrivateMethods

    }
}
