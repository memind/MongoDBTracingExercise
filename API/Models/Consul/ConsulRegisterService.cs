using Consul;
using Microsoft.Extensions.Options;

namespace API.Models.Consul
{
    public class ConsulRegisterService : IHostedService
    {
        private IConsulClient _client;
        private MenuConfiguration _menuConfig;
        public ConsulRegisterService(IConsulClient client, IOptions<MenuConfiguration> menuConfig)
        {
            _client = client;
            _menuConfig = menuConfig.Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var menuUri = new Uri(_menuConfig.Url);
            var serviceRegistration = new AgentServiceRegistration()
            {
                Address = menuUri.Host,
                Name = _menuConfig.ServiceName,
                Port = menuUri.Port,
                ID = _menuConfig.ServiceId,
                Tags = new[] {_menuConfig.ServiceName}
            };
            await _client.Agent.ServiceDeregister(_menuConfig.ServiceId, cancellationToken);
            await _client.Agent.ServiceRegister(serviceRegistration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_menuConfig.ServiceId, cancellationToken);
        }
    }
}
