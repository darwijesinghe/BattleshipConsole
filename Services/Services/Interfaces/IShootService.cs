using Domain.Models;
using Services.Response;

namespace Services.Services.Interfaces
{
    /// <summary>
    /// Defines contracts of shoot handling related functionalities.
    /// </summary>
    public interface IShootService
    {
        /// <summary>
        /// Retrieves the result of a shot based on the provided position.
        /// </summary>
        /// <param name="position">The position of the shot as a <see cref="ShootPosition"/> object.</param>
        /// <returns>
        /// The <see cref="Result{ShootResult}"/> indicating the outcome of the shot.
        /// </returns>
        Task<Result<ShootResult>> ShootResult(ShootPosition position);
    }
}
