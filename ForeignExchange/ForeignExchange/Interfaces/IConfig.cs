using SQLite.Net.Interop;

namespace ForeignExchange.Interfaces
{
    public interface IConfig
    {
        string DirectoryDB { get; }
        ISQLitePlatform Platform { get; }
    }
}
