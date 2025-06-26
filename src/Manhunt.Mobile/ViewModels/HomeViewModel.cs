using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Manhunt.Mobile.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public ICommand GoToLoginCommand { get; }

        public HomeViewModel()
        {
            GoToLoginCommand = new Command(async () =>
            {
                // navigiere zur LoginPage
                await Shell.Current.GoToAsync("LoginPage");
            });
        }
    }
}
