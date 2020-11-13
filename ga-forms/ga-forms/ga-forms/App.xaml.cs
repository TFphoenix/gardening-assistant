using System;
using ga_forms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ga_forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyInjectionManager.Init();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
