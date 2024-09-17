using Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Response;
using Services.Services.Interfaces;
using System.Net;

namespace Services.Services.Classes
{
    public class ShootService : IShootService
    {
        // services
        private ILogger<ShootService>   _logger;
        private readonly HttpClient _httpClient;

        public ShootService(ILogger<ShootService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger     = logger;
            _httpClient = httpClientFactory.CreateClient("GlobalClient");
        }

        /// <summary>
        /// Holds the HTTP response message
        /// </summary>
        private HttpResponseMessage? _response { get; set; }

        /// <summary>
        /// Retrieves the result of a shot based on the provided position
        /// </summary>
        /// <param name="position">The position of the shot as a <see cref="ShootPosition"/> object</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> indicating the outcome of the shot
        /// </returns>
        public async Task<Result<ShootResult>> ShootResult(ShootPosition position)
        {
            try
            {
                // adds custom request header
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-consumer", "console-app");

                // creates query string with proper encoding
                var query = $"?row={Uri.EscapeDataString(position.Row.ToString())}&column={Uri.EscapeDataString(position.Column.ToString())}";

                // calling to the endpoint
                using (_response = await _httpClient.GetAsync($"Shoots/ShootResult{query}"))
                {
                    // checks the response status
                    if (_response.StatusCode != HttpStatusCode.OK)
                        return new Result<ShootResult> { Message = _response.ReasonPhrase, Success = false };

                    // reads the response
                    string result = await _response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(result))
                    {
                        // deserializing the result
                        var data = JsonConvert.DeserializeObject<Result<ShootResult>>(result) ?? new Result<ShootResult>();
                        if (data.Success)
                            // returns the result
                            return new Result<ShootResult> { Success = data.Success, Data = data.Data };
                        else
                            return new Result<ShootResult> { Message = "Error occurred." };
                    }

                    // returns the result
                    return new Result<ShootResult> { Message = "Error occurred." };
                }
            }
            catch (HttpRequestException ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
