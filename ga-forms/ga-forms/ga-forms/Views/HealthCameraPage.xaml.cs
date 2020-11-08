using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
}