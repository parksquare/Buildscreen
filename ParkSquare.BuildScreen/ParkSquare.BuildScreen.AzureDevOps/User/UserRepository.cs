using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParkSquare.BuildScreen.AzureDevOps.User.Dto;

namespace ParkSquare.BuildScreen.AzureDevOps.User
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IUserRepository> _logger;
        private readonly IAzureDevOpsConfig _config;

        private IReadOnlyCollection<AzureUser> _users;

        public UserRepository(IHttpClientFactory httpClientFactory, ILogger<IUserRepository> logger, IAzureDevOpsConfig config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<AzureUser> GetUserFromEmailAsync(string email)
        {
            // Need to add locking here
             
            if (_users == null || !_users.Any())
            {
                _users = await GetUsersAsync();
            }

            var match = _users.FirstOrDefault(x =>
                x.EmailAddress.Equals(email, StringComparison.InvariantCultureIgnoreCase));

            return match;
        }

        private async Task<List<AzureUser>> GetUsersAsync()
        {
            // Should use X-MS-ContinuationToken here, and handling paging correctly

            var client = _httpClientFactory.GetJsonClient();
            var requestUri = new Uri($"https://vssps.dev.azure.com/{_config.Organization}/_apis/graph/users?api-version=5.1-preview.1");

            try
            {
                using (var response = await client.GetAsync(requestUri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var deserialized = await DeserializeResponseAsync(response);
                        return ConvertUsers(deserialized.Value).ToList();
                    }
                    else
                    {
                        _logger.LogError($"Azure Dev Ops API returned {response.StatusCode} {response.ReasonPhrase}");

                        throw new AzureDevOpsProviderException(
                            "Unable to get user. " +
                            $"Call to '{requestUri}' returned {response.StatusCode}: {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Error getting users for organization '{_config.Organization}' from Azure Dev Ops API");
            }

            return new List<AzureUser>(); 
        }

        private IEnumerable<AzureUser> ConvertUsers(IReadOnlyCollection<UserDto> userDtos)
        {
            return userDtos.Select(ConvertUser);
        }

        public AzureUser ConvertUser(UserDto userDto)
        {
            return new AzureUser
            {
                Descriptor = userDto.Descriptor, 
                DisplayName = userDto.DisplayName, 
                EmailAddress = userDto.MailAddress,
                PrincipalName = userDto.PrincipalName
            };
        }

        private static async Task<GetUsersResponseDto> DeserializeResponseAsync(HttpResponseMessage response)
        {
            var deserialized = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetUsersResponseDto>(deserialized);
        }
    }
}