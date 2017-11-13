using Xamarin.Forms;

[assembly:Dependency(typeof(ForeignExchange.iOS.Implementations.Config))]
namespace ForeignExchange.iOS.Implementations
{
    using ForeignExchange.Interfaces;
    using SQLite.Net.Interop;

    class Config : IConfig
    {
        string directoryDB;

        ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    var directory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                    directoryDB = System.IO.Path.Combine(directory, "..", "Library");
                }
                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform  == null)
                {
                    platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
                }
                return platform;
            }
        }
    }
}