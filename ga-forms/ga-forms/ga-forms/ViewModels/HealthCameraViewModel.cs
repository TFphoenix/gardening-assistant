using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Views;
using Xamarin.Forms;

namespace ga_forms.ViewModels
{
    class HealthCameraViewModel : ViewModel
    {
        public Command GoBackCommand { get; }
        public Command UploadCommand { get; }

        public HealthCameraViewModel()
        {
            Title = "Health Camera Page";
            GoBackCommand = new Command(OnBack);
            UploadCommand = new Command(UploadImage);
        }

        public async void TakeSnapshot()
        {
            // TODO: Revieve image
            await Shell.Current.GoToAsync($"//{nameof(HealthSelectionPage)}");
        }

        private void UploadImage(object obj)
        {
            //TODO
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
