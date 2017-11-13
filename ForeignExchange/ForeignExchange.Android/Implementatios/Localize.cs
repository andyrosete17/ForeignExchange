using Xamarin.Forms;

[assembly: Dependency(typeof(ForeignExchange.Droid.Implementatios.Localize))] //Indicate who is implementing the localize interface method for iOS

namespace ForeignExchange.Droid.Implementatios
{
    using ForeignExchange.Interfaces;
    using Helpers;
    using System.Globalization;
    using System.Threading;

    public class Localize : ILocalize
    {
        public CultureInfo GetCurrentCultureInfo()
        {
            var netLanguage = "en";
            var androidLocale = Java.Util.Locale.Default;
            netLanguage = AndroidToDotnetLanguage(androidLocale.ToString().Replace("_", "-"));
            CultureInfo ci = null;
            try
            {
                ci = new CultureInfo(netLanguage);
            }
            catch (CultureNotFoundException ei)
            {
                ei.ToString();
                //iOS locale not valid .NET culture  (eg. "en-ES) : English in Spain
                //fallback to first characters, in this case "en"
                try
                {
                    var fallback = ToDotnetFallbackLanguage(new PlatformCulture(netLanguage));
                    ci = new CultureInfo(fallback);
                }
                catch (CultureNotFoundException e2)
                {
                    e2.ToString();
                    //iOSlanguage not valid .NET culture, falling back to english
                    ci = new CultureInfo("en");
                }
            }
            return ci;
        }

        public void SetLocale(CultureInfo ci)
        {
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        string AndroidToDotnetLanguage(string androidLanguage)
        {
            var netLanguage = androidLanguage;
            //certain language need to be converted to CultureInfo equivalent
            switch (androidLanguage)
            {
                case "ms-BN"://Malaysian Brunei not supported .NET culture
                case "ms-MY": //Malaysian Malaysia not supported .NET culture
                case "ms-SG"://Malaysian Singapur not supported .NET culture
                    netLanguage = "ms";
                    break;
                case "gsw-CH": //Swiss German not supported .NET culture
                    netLanguage = "de-CH";
                    break;
                case "in-ID": //Indonesian has different code in .NET
                    netLanguage = "id-ID";
                    break;
            }
            return netLanguage;
        }

        string ToDotnetFallbackLanguage(PlatformCulture platCulture)
        {
            var netLanguage = platCulture.LanguageCode; // use the first part of the identifier (two chars, usually)
            switch (platCulture.LanguageCode)
            {
                case "gsw":
                    netLanguage = "de-CH"; // Equivalent to german 
                    break;
            }
            return netLanguage;
        }
    }
}