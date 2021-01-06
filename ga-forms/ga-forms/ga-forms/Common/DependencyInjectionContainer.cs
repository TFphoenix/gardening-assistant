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
            // Register Services
            services.AddSingleton<IDialogBoxService, DialogBoxService>();
            services.AddSingleton<IImageManagerService, ImageManagerService>();

            // Register ViewModels that use dependency injection
            services.AddTransient<HealthResultsViewModel>();
            services.AddTransient<HealthCameraViewModel>();
            services.AddTransient<HealthSelectionViewModel>();
            services.AddTransient<DecorateViewModel>();

            return services;
        }
    }
}
