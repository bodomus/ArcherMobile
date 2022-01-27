using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using ArcherMobilApp.BLL.Contracts;
using ArcherMobilApp.BLL.Models;


namespace ArcherMobilApp.BLL
{
    public class UsersMockProxyService: IUsersMockProxyService, IHealthCheck
    {
        private HttpClient _client { get; }

        public UsersMockProxyService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var response = await _client.GetAsync("");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadAsAsync<IEnumerable<User>>();
            return result;
        }

        public async Task<User> GetUserById(int id)
        {
            var response = await _client.GetAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var result = await response.Content.ReadAsAsync<User>();
            return result;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _client.BaseAddress));
                response.EnsureSuccessStatusCode();
                return HealthCheckResult.Healthy();
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }

    }
}
