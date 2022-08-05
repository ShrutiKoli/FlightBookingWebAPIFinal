using Consul;
using IApplicationLifetime = Microsoft.AspNetCore.Hosting.IApplicationLifetime;

namespace BookingManagementService.Config
{
    public static class ServiceRegistryAppExtension
    {
        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            string consulAddress = configuration.GetValue<string>("ConsulConfig:Host");
            services.AddSingleton<IConsulClient, ConsulClient>(p =>
            new ConsulClient(consulconfig =>
            {
                consulconfig.Address = new Uri(consulAddress);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app, IConfiguration configuration)
        {

            var consulclient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtenstion");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();
            var servicename = configuration.GetValue<string>("ConsulConfig:ServiceName");
            var serviceid = configuration.GetValue<string>("ConsulConfig:ServiceId");



            var registration = new AgentServiceRegistration()
            {
                ID = serviceid,
                Name = servicename,
                Address = "localhost",
                Port = 7013
            };
            logger.LogInformation("Registering");
            consulclient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            consulclient.Agent.ServiceRegister(registration).ConfigureAwait(true);
            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering");
                consulclient.Agent.ServiceDeregister(serviceid).ConfigureAwait(true);

            });

            return app;
        }
    }
}
