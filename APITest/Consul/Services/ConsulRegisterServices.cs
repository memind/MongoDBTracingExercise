using APITest.Consul.Configs;
using Consul;
using Microsoft.Extensions.Options;

namespace APITest.Consul.Services
{
    public class ConsulRegisterServices : IHostedService
    {
        private IConsulClient _client;
        private TestServiceConfiguration _test1Config;
        private Test2ServiceConfiguration _test2Config;

        public ConsulRegisterServices(IOptions<TestServiceConfiguration> testConfig, IConsulClient client, IOptions<Test2ServiceConfiguration> test2Config)
        {
            _test1Config = testConfig.Value;
            _client = client;
            _test2Config = test2Config.Value;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var test1Uri = new Uri(_test1Config.Url);

            var test1Registration = new AgentServiceRegistration()
            {
                Address = test1Uri.Host,
                Name = _test1Config.ServiceName,
                ID = _test1Config.ServiceId,
                Port = test1Uri.Port,
                Tags = new[] { _test1Config.ServiceName }
            };

            var test2Uri = new Uri(_test2Config.Url);

            var test2Registration = new AgentServiceRegistration()
            {
                Address = test2Uri.Host,
                Name = _test2Config.ServiceName,
                ID = _test2Config.ServiceId,
                Port = test2Uri.Port,
                Tags = new[] { _test2Config.ServiceName }
            };

            await _client.Agent.ServiceDeregister(_test1Config.ServiceId, cancellationToken);
            await _client.Agent.ServiceDeregister(_test2Config.ServiceId, cancellationToken);

            await _client.Agent.ServiceRegister(test1Registration, cancellationToken);
            await _client.Agent.ServiceRegister(test2Registration, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.Agent.ServiceDeregister(_test1Config.ServiceId, cancellationToken);
            await _client.Agent.ServiceDeregister(_test2Config.ServiceId, cancellationToken);
        }
    }
}
