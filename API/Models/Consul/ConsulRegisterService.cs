using API.Extensions;
using Consul;
using Microsoft.Extensions.Options;

namespace API.Models.Consul
{
    public class ConsulRegisterService : IHostedService
    {
        private IConsulClient _client;
        private WorkoutConfiguration _workoutConfiguration;
        public ConsulRegisterService(IConsulClient client, IOptions<WorkoutConfiguration> workoutConfig)
        {
            _client = client;
            _workoutConfiguration = workoutConfig.Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var menuUri = new Uri(_workoutConfiguration.Url);
            var serviceRegistration = new AgentServiceRegistration()
            {
                Address = menuUri.Host,
                Name = _workoutConfiguration.ServiceName,
                Port = menuUri.Port,
                ID = _workoutConfiguration.ServiceId,
                Tags = new[] {_workoutConfiguration.ServiceName}
            };
            await _client.Agent.ServiceDeregister(_workoutConfiguration.ServiceId, cancellationToken);
            await _client.Agent.ServiceRegister(serviceRegistration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_workoutConfiguration.ServiceId, cancellationToken);
        }
    }
}
