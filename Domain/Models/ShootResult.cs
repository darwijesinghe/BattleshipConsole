using Domain.Enums;

namespace Domain.Models
{
    /// <summary>
    /// Domian class for the shoot result
    /// </summary>
    public class ShootResult
    {
        public ShootResult()
        {
            ShootHistory = new List<ShootPosition>();
            DamagedShip  = string.Empty;
        }

        /// <summary>
        /// Type of shoot
        /// </summary>
        public ShootStatus ShootStatus          { get; set; }

        /// <summary>
        /// Ship that was damaged
        /// </summary>
        public string DamagedShip               { get; set; }

        /// <summary>
        /// Shoot history and shooted positions
        /// </summary>
        public List<ShootPosition> ShootHistory { get; set; }

    }
}
