using Consul;
using Microsoft.Extensions.Options;

namespace API.Extensions
{
    public class ConsulRegisterServices : IHostedService
    {
        private IConsulClient _client;
        private WorkoutConfiguration _workoutConfig;

        public ConsulRegisterServices(IOptions<WorkoutConfiguration> workoutConfig, IConsulClient client)
        {
            _workoutConfig = workoutConfig.Value;
            _client = client;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var workoutUri = new Uri(_workoutConfig.Url);

            var workoutRegistration = new AgentServiceRegistration()
            {
                Address = workoutUri.Host,
                Name = _workoutConfig.ServiceName,
                ID = _workoutConfig.ServiceId,
                Port = workoutUri.Port,
                Tags = new[] { _workoutConfig.ServiceName }
            };


            await _client.Agent.ServiceDeregister(_workoutConfig.ServiceId, cancellationToken);

            await _client.Agent.ServiceRegister(workoutRegistration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_workoutConfig.ServiceId, cancellationToken);
        }
    }
}
