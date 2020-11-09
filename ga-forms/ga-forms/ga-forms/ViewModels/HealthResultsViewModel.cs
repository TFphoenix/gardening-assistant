using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthResultsViewModel : ViewModel
    {
        public Command GoBackCommand { get; }
        public Command GoHomeCommand { get; }
        public Command SaveCommand { get; }
        public HealthResultsViewModel()
        {
            Title = "Health Results Page";
            GoBackCommand = new Command(OnBack);
            GoHomeCommand = new Command(OnHome);
            SaveCommand = new Command(OnSave);
        }
        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthCameraPage)}");
        }
        private async void OnHome(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        private async void OnSave(object obj)
        {
            // todo
        }
    }
}
