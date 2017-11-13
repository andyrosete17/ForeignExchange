
namespace ForeignExchange.ViewModels
{
    using ForeignExchange.ApiServices;
    #region Using
    using ForeignExchange.Helpers;
    using ForeignExchange.Models;
    using ForeignExchange.Services;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System;
    using System.Threading.Tasks;
    #endregion Using
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

        #region Services
        ApiService apiService;
        DialogService dialogService;
        DataService dataService;
        #endregion Services

        #region Attributes
        bool _isRunning;
        string _result;
        bool _isEnable;
        Rate _sourceRate;
        Rate _targetRate;
        List<Rate> rates;
        ObservableCollection<Rate> _rates;
        string _status;
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
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    PropertyChanged?.Invoke
                        (
                            this,
                            new PropertyChangedEventArgs(nameof(Status))
                        );
                }
            }
        }
        #endregion Properties

        #region Constructors
        public MainViewModel()
        {
            apiService = new ApiService();
            dialogService = new DialogService();
            dataService = new DataService();
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
                await dialogService.ShowMessage(
                        Languages.Error,
                        Languages.AmountValidation
                        );
                return;
            }
            decimal amount = 0;
            if (!decimal.TryParse(Amount, out amount))
            {
                await dialogService.ShowMessage
                                    (
                                        Languages.Error,
                                        Languages.AmountNumericValidation
                                        );
                return;
            }
            if (SourceRate == null)
            {
                await dialogService.ShowMessage
                                   (
                                        Languages.Error,
                                        Languages.SourceRateValidation
                                       );
                return;
            }

            if (TargetRate == null)
            {
                await dialogService.ShowMessage
                                   (
                                        Languages.Error,
                                        Languages.TargetRateValidation
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
            Result = Languages.Loading;

            var connection = await apiService.CheckConnection();
            if (connection.isSucess)
            {
                await LoadDataFromApi();
            }
            else
            {
                LoadLocalData();
            }

            if (rates.Count == 0)
            {
                IsRunning = false;
                IsEnable = false;
                Result = Languages.InternetErrorNoLocalData;
                Status = Languages.NoRatesLoaded;
                return;
            }

            Rates = new ObservableCollection<Rate>(rates);
            IsRunning = false;
            IsEnable = true;
            Result = Languages.Ready;
        }

        private void LoadLocalData()
        {
            rates = dataService.Get<Rate>(false);
            Status = Languages.RatesLoadedLocal;
        }

        private async Task LoadDataFromApi()
        {
            var url = Application.Current.Resources["URLAPI"].ToString();
            var urlRate = Application.Current.Resources["URLRATES"].ToString();
            var response = await apiService.GetList<Rate>(url, urlRate);
            if (!response.isSucess)
            {
                LoadLocalData();
                return;
            }

            //Storage data local
            rates = (List<Rate>)response.Result;
            dataService.DeleteAll<Rate>();
            dataService.Save(rates);

            Status = Languages.RatesLoadedInternet;
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
