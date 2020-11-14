using System;
using System.Collections.Generic;
using System.Text;
using ga_forms.Services;
using ga_forms.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ga_forms.Common
{
    public static class DependencyInjectionContainer
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<IDialogBoxService, DialogBoxService>();
            services.AddTransient<HealthResultsViewModel>();

            return services;
        }
    }
}
