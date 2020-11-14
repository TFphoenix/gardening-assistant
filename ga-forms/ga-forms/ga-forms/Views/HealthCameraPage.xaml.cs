using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ga_forms.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ga_forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthCameraPage : ContentPage
    {
        public HealthCameraPage()
        {
            InitializeComponent();
            CameraButton.Margin = new Thickness(0, 0, 0, 30);
        }

        private void CameraView_OnAvailable(object sender, bool e)
        {
            CameraButton.IsEnabled = e;
        }

        private void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            //TODO: Export image
            //e.Image
            (BindingContext as HealthCameraViewModel)?.TakeSnapshot();
        }

        private void CameraView_MediaCaptureFailed(object sender, string e)
        {
            //TODO: Error handling
        }
    }
}