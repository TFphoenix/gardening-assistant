using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ga_forms.Common;
using ga_forms.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ga_forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DecoratePage : ContentPage
    {
        public DecoratePage()
        {
            InitializeComponent(); 
            CameraButton.Margin = new Thickness(0, 0, 0, 30);
        }
        private void CameraView_OnAvailable(object sender, bool e)
        {
            CameraButton.IsEnabled = e;
        }

        private void CameraView_MediaCaptured(object sender, MediaCapturedEventArgs args)
        {
            (BindingContext as HealthCameraViewModel)?.OnSnapshot(args.Path);
        }

        private void CameraView_MediaCaptureFailed(object sender, string e)
        {
            //TODO: Handle error
        }
    }
}