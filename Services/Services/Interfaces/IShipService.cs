using Services.Response;

namespace Services.Services.Interfaces
{
    public interface IShipService
    {
        /// <summary>
        /// Calls the API to place ships and returns the result
        /// </summary>
        /// <returns>
        /// The <see cref="Result"/> indicating the outcome of the API call
        /// </returns>
        Task<Result> PlaceShips();
    }
}
