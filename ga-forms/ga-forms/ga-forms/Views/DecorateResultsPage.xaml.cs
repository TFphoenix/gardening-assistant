using ga_forms.Common;
using ga_forms.ViewModels;
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
    public partial class DecorateResultsPage : ContentPage
    {
        public DecorateResultsPage()
        {
            InitializeComponent();
            BindingContext = DependencyInjectionManager.ServiceProvider.GetService<DecorateResultsViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}