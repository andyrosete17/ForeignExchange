﻿
namespace ForeignExchange.Helpers
{
    using ForeignExchange.Interfaces;
    using ForeignExchange.Resources;
    using Xamarin.Forms;

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }
        public static string AmountLabel
        {
            get { return Resource.AmountLabel; }
        }
        public static string AmountNumericValidation
        {
            get { return Resource.AmountNumericValidation; }
        }
        public static string AmountPlaceHolder
        {
            get { return Resource.AmountPlaceHolder; }
        }
        public static string Convert
        {
            get { return Resource.Convert; }
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string Loading
        {
            get { return Resource.Loading; }
        }
        public static string Ready
        {
            get { return Resource.Ready; }
        }
        public static string SourceRateLabel
        {
            get { return Resource.SourceRateLabel; }
        }
        public static string SourceRateTitle
        {
            get { return Resource.SourceRateTitle; }
        }
        public static string SourceRateValidation
        {
            get { return Resource.SourceRateValidation; }
        }
        public static string TargetRateLabel
        {
            get { return Resource.TargetRateLabel; }
        }
        public static string TargetRateTitle
        {
            get { return Resource.TargetRateTitle; }
        }
        public static string TargetRateValidation
        {
            get { return Resource.TargetRateValidation; }
        }
        public static string Title
        {
            get { return Resource.Title; }
        }
        public static string AmountValidation
        {
            get { return Resource.AmountValidation; }
        }
        public static string InternetError
        {
            get { return Resource.InternetError; }
        }
        public static string InternetSettingsError
        {
            get { return Resource.InternetSettingsError; }
        }
        public static string RatesLoadedInternet
        {
            get { return Resource.RatesLoadedInternet; }
        }
        public static string InternetErrorNoLocalData
        {
            get { return Resource.InternetErrorNoLocalData; }
        }
        public static string RatesLoadedLocal
        {
            get { return Resource.RatesLoadedLocal; }
        }
        public static string NoRatesLoaded
        {
            get { return Resource.NoRatesLoaded; }
        }
        

    }
}
