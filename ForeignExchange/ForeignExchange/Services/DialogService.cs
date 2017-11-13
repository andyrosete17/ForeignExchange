
namespace ForeignExchange.Services
{
    using ForeignExchange.Helpers;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class DialogService
    {
        public async Task ShowMessage(string Title, string message)
        {
            await Application.Current.MainPage.DisplayAlert(Title, message, Languages.Accept);
        }
    }
}
