namespace App.Helpers
{
    /// <summary>
    /// Class for the helper mothods.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Checks whether the given <see cref="IEnumerable{T}"/> has any non-null elements
        /// </summary>
        /// <param name="data">The collection of elements to check for non-null values</param>
        /// <returns>
        /// <c>true</c> if the collection is not null and contains at least one non-null element; otherwise, <c>false</c>
        /// </returns>
        public static bool HasValue<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }

        /// <summary>
        /// Converts a number to its corresponding uppercase letter in the alphabet
        /// For example, 1 = 'A', 2 = 'B', ..., 8 = 'H'.
        /// </summary>
        /// <param name="number">The number to convert to a letter (1-10)</param>
        /// <returns>
        /// The corresponding uppercase letter for the given number
        /// </returns>

        public static string LetterFromNumber(int number)
        {
            try
            {
                string result = string.Empty;
                switch (number)
                {
                    case 1: result = "A"; break;
                    case 2: result = "B"; break;
                    case 3: result = "C"; break;
                    case 4: result = "D"; break;
                    case 5: result = "E"; break;
                    case 6: result = "F"; break;
                    case 7: result = "G"; break;
                    case 8: result = "H"; break;
                    case 9: result = "I"; break;
                    case 10: result = "J"; break;
                    default:
                        break;
                }

                // returns the correct letter for the number
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Converts a single uppercase letter (A-J) to its corresponding number in the alphabet
        /// For example, 'A' = 1, 'B' = 2, ..., 'H' = 8
        /// </summary>
        /// <param name="letter">The uppercase letter to convert (A-J).</param>
        /// <returns>
        /// The corresponding number for the given letter
        /// </returns>

        public static int NumberFromLetter(string letter)
        {
            try
            {
                int result = 0;
                switch (letter.ToUpper())
                {
                    case "A": result = 1; break;
                    case "B": result = 2; break;
                    case "C": result = 3; break;
                    case "D": result = 4; break;
                    case "E": result = 5; break;
                    case "F": result = 6; break;
                    case "G": result = 7; break;
                    case "H": result = 8; break;
                    case "I": result = 9; break;
                    case "J": result = 10; break;
                    default:
                        break;
                }

                // returns the correct number for the letter
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
