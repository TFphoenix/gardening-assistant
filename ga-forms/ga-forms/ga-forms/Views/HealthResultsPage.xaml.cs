using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ga_forms.Common;
using ga_forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ga_forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthResultsPage : ContentPage
    {
        public HealthResultsPage()
        {
            InitializeComponent();
            BindingContext = DependencyInjectionManager.ServiceProvider.GetService<HealthResultsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as HealthResultsViewModel)?.OnAppearing();
        }
    }
}