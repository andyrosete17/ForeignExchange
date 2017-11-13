using Xamarin.Forms;

[assembly :Dependency(typeof(ForeignExchange.Droid.Implementatios.Config))]
namespace ForeignExchange.Droid.Implementatios
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
                    directoryDB = System.Environment.GetFolderPath
                        (System.Environment.SpecialFolder.Personal);
                }
                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                   // SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid platformTemp = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                    platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                    //platform = platformTemp;
                }
                return platform;
            }
        }

    }
}