using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Response;
using Services.Services.Interfaces;
using System.Net;

namespace Services.Services.Classes
{
    public class ShipService : IShipService
    {
        // services
        private ILogger<ShipService>    _logger;
        private readonly HttpClient _httpClient;

        public ShipService(ILogger<ShipService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger     = logger;
            _httpClient = httpClientFactory.CreateClient("GlobalClient");
        }

        /// <summary>
        /// Holds the HTTP response message
        /// </summary>
        private HttpResponseMessage? _response { get; set; }

        /// <summary>
        /// Calls the API to place ships and returns the result
        /// </summary>
        /// <returns>
        /// The <see cref="Result"/> indicating the outcome of the API call
        /// </returns>
        public async Task<Result> PlaceShips()
        {
            try
            {
                // adds custom request header
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-consumer", "console-app");

                // calling to the endpoint
                using (_response = await _httpClient.GetAsync("Ships/PlaceShips"))
                {
                    // checks the response status
                    if (_response.StatusCode != HttpStatusCode.OK)
                        return new Result { Message = _response.ReasonPhrase, Success = false };

                    // reads the response
                    string result = await _response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(result))
                    {
                        // deserializing the result
                        var data = JsonConvert.DeserializeObject<Result>(result) ?? new Result();
                        if (data.Success)
                            // returns the result
                            return new Result { Success = data.Success };
                        else
                            return new Result { Message = "Error occurred." };
                    }

                    // returns the result
                    return new Result { Message = "Error occurred." };
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
