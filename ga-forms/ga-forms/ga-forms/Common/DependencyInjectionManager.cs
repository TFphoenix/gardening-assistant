using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ga_forms.Common
{
    public class DependencyInjectionManager
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static IServiceProvider Init()
        {
            var serviceProvider = new ServiceCollection().ConfigureServices().BuildServiceProvider();
            ServiceProvider = serviceProvider;

            return serviceProvider;
        }
    }
}
