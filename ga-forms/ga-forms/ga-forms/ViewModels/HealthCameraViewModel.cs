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
        public Command TakeSnapshotCommand { get; }
        public Command UploadCommand { get; }

        public HealthCameraViewModel()
        {
            Title = "Health Camera Page";
            GoBackCommand = new Command(OnBack);
            TakeSnapshotCommand = new Command(TakeSnapshot);
            UploadCommand = new Command(UploadImage);
        }

        private void UploadImage(object obj)
        {
            //TODO
        }

        private void TakeSnapshot(object obj)
        {
            //TODO
        }

        private async void OnBack(object obj)
        {
            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
    }
}
