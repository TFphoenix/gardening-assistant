using ga_forms.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class DecorateResultsViewModel : ViewModel
    {
        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }

        public DecorateResultsViewModel()
        {
            Title = "Health Results Page";
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
        }
        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(DecorateSelectionPage)}");
        }
        private async void OnHome(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
