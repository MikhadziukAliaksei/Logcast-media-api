using Logcast.Recruitment.DataAccess.Configuration;
using Logcast.Recruitment.Domain.Configuration;
using Logcast.Recruitment.Shared.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Logcast.Recruitment.Web.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = new AppSettings(configuration);
            services.AddSingleton<IAppSettings>(appSettings);
            services.AddEntityFramework(configuration);

            services.AddDataAccessServices();
            services.AddDomainServices();
        }
    }
}