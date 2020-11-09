using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthSelectionViewModel : ViewModel
    {
        public Command GoBackCommand { get; }
        public Command SaveCommand { get; }
        public HealthSelectionViewModel()
        {
            Title = "Health Selection Page";
            GoBackCommand = new Command(OnBack);
            SaveCommand = new Command(OnSave);
        }
        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthCameraPage)}");
        }
       
        private async void OnSave(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HealthResultsPage)}"); 
        }
    }
}
