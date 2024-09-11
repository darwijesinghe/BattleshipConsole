using App.Helpers;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Logging;
using Services.Services.Interfaces;

namespace App
{
    public class Startup
    {
        // services
        private readonly ILogger<Startup>    _logger;
        private readonly IShipService   _shipService;
        private readonly IShootService _shootService;

        public Startup(ILogger<Startup> logger, IShipService shipService, IShootService shootService)
        {
            _logger       = logger ?? throw new ArgumentNullException(nameof(logger));
            _shipService  = shipService;
            _history      = new ShootResult();
            _shootService = shootService;
        }

        /// <summary>
        /// Shoot history
        /// </summary>
        private ShootResult _history { get; set; }

        public async Task Run() 
        {
            // startup
            await Start();
        }

        /// <summary>
        /// Main function. This will handle all the sub processes
        /// </summary>
        private async Task Start()
        {
            try
            {
                // prits the header
                PrintHeader();

                string answer;

                do
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Please press [S] to start or [E] to exit:");
                    Console.Write(" ");

                    // reads the user input
                    answer = Console.ReadLine()?.ToUpper()?.Trim() ?? string.Empty;

                    switch (answer)
                    {
                        case "S":
                            // call the ship place api and draw the board if response is ok
                            var result = await ShowProcessingWhileTaskCompletes(PlaceShipsOnBoard);
                            if (result)
                            {
                                do
                                {
                                    // reset the board
                                    // draw the play board
                                    // ask the first shot
                                    //  : call the shoot endpoint
                                    //  : reset the board
                                    //  : redraw the board with history
                                    //  : show the shot result

                                    ResetBoard();
                                    DrawBoard(_history);
                                    await Shoot();
                                    ResetBoard();
                                    DrawBoard(_history);
                                    ShotStatus();

                                    if (_history.ShootStatus != ShootStatus.Won)
                                    {
                                        Console.Write("Press any key to continue...");
                                        Console.ReadLine();
                                    }

                                } while (_history.ShootStatus != ShootStatus.Won);
                            }
                            break;

                        case "E":
                            // exit the game
                            await Exit();
                            break;

                        default:
                            // invalid input
                            PrintError("Invalid input.");
                            break;
                    }

                } while (!answer.Equals("S") && !answer.Equals("E"));
            }
            catch (Exception ex)
            {
                // prints the error
                PrintError("Error, Please try again.");

                // logs the exception
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Prints the main header
        /// </summary>
        private void PrintHeader()
        {
            try
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("------------------------------");
                Console.WriteLine("Battleship Assessment WireApps");
                Console.WriteLine("------------------------------");
                Console.WriteLine();
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Places ships on the game board, ensuring valid positions
        /// </summary>
        /// <returns>
        /// The task result is true if the ships were placed successfully, otherwise false
        /// </returns>

        private async Task<bool> PlaceShipsOnBoard()
        {
            try
            {
                // gets the response
                var result = await _shipService.PlaceShips();
                if (result.Success)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Draws the current game board based on the shooting history.
        /// Displays the positions where ships were hit, miss or sunk on the board.
        /// </summary>
        /// <param name="history">The history of the shooting results including hit, miss, invalid, same or sunk positions</param>

        private void DrawBoard(ShootResult history)
        {
            try
            {
                // shot outcomes
                ShotOutcome();

                // sample grid

                //   1 2 3 4 5 6 7 8 9 10
                //A  | | | | | | | | | |
                //B  | | | | | | | | | |
                //C  | | | | | | | | | |
                //D  | | | | | | | | | |
                //E  | | | | | | | | | |
                //F  | | | | | | | | | |
                //G  | | | | | | | | | |
                //H  | | | | | | | | | |
                //I  | | | | | | | | | |
                //J  | | | | | | | | | |

                // renders horizontal numbers. 1 to 10
                Console.Write("  ");
                for (int y = 1; y <= 10; y++)
                {
                    Console.Write(y);
                    Console.Write(" ");
                }
                Console.WriteLine();

                // renders vertical letters. A to J
                for (int x = 1; x <= 10; x++)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(Helper.LetterFromNumber(x) + " ");
                    Console.ResetColor();

                    for (int y = 1; y <= 10; y++)
                    {
                        // gets the shoot result
                        // marks the letter accordingly

                        var status = GetShootStatus(new ShootPosition(x, y));
                        switch (status)
                        {
                            case ShootStatus.Invalid:
                                Console.Write("I");
                                break;
                            case ShootStatus.Same:
                                Console.Write("D");
                                break;
                            case ShootStatus.Miss:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("M");
                                Console.ResetColor();
                                break;
                            case ShootStatus.Hit:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("H");
                                Console.ResetColor();
                                break;
                            case ShootStatus.Sunk:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("S");
                                Console.ResetColor();
                                break;
                            default:
                                Console.Write(" ");
                                break;
                        }

                        Console.Write("|");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a shooting action in the game based on user input.
        /// It sends the shooting request to the server, processes the result, and updates the game state accordingly.
        /// </summary>
        private async Task Shoot()
        {
            try
            {
                string input;
                do
                {
                    Console.Write("\r" + new string(' ', Console.WindowWidth));
                    Console.Write("\rEnter shooting position:");
                    Console.Write(" ");

                    // reads the user input
                    input = Console.ReadLine()?.Trim() ?? string.Empty;

                    // converts user input to the correct position
                    var positon = PrepareShootPosition(input);

                    // gets the shoot result and stores it in a variable
                    var result = await _shootService.ShootResult(positon);
                    if (result.Success)
                    {
                        if (result.Data is not null)
                            _history = result.Data;
                    }
                    else
                        PrintError(result.Message);

                } while (string.IsNullOrEmpty(input));
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Updates the status of the shot
        /// </summary>
        private void ShotStatus()
        {
            try
            {
                // gets the shoot result from the history
                var status      = _history.ShootStatus;
                var damagedShip = string.IsNullOrEmpty(_history.DamagedShip) ? " - " : _history.DamagedShip;

                Console.WriteLine();
                Console.Write($"\rShoot status: {status}    Damaged ship: {damagedShip}");
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Prepares a shoot position based on the given string
        /// </summary>
        /// <param name="position">The position as a string (e.g., "A1", "B5")</param>
        /// <returns>
        /// A <see cref="ShootPosition"/> object representing the prepared position
        /// </returns>
        private ShootPosition PrepareShootPosition(string position)
        {
            try
            {
                // validates before proceed
                if (!string.IsNullOrEmpty(position) && position.Length > 0)
                {
                    // prepares the shoot positions
                    // ex: A1

                    int row, column;
                    var letter = position.Substring(0, 1);
                    row        = Helper.NumberFromLetter(letter);
                    if (row > 0 && int.TryParse(position.Substring(1), out column))
                    {
                        return new ShootPosition(row, column);
                    }
                }
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }

            // returns default shot
            return new ShootPosition(0, 0);
        }

        /// <summary>
        /// Retrieves the status of a shot based on the given shoot position
        /// </summary>
        /// <param name="position">The position of the shot as a <see cref="ShootPosition"/> object</param>
        /// <returns>
        /// The status of the shot as a <see cref="ShootStatus"/> object
        /// </returns>
        private ShootStatus GetShootStatus(ShootPosition position)
        {
            // checks the current hit position contains in the history
            // returns the shoot status if it contains
            // otherwise returns default status

            if (_history.ShootHistory.Contains(position))
            {
                return _history.ShootHistory.FirstOrDefault(s => s.Row == position.Row && s.Column == position.Column)?.ShootStatus ?? ShootStatus.NotSet;
            }

            return ShootStatus.NotSet;
        }

        /// <summary>
        /// Executes a task while displaying a processing indicator
        /// </summary>
        /// <typeparam name="T">The type of the result returned by the task</typeparam>
        /// <param name="task">A function that returns a Task of type T to be executed</param>
        /// <returns>
        /// A Task of type T representing the result of the executed task
        /// </returns>
        private async Task<T> ShowProcessingWhileTaskCompletes<T>(Func<Task<T>> task)
        {
            try
            {
                // ingredients
                var spinner    = new[] { "|", "/", "-", "\\" };
                int counter    = 0;

                // starts the long-running task in the background
                var taskRunning = task();

                // shows the processing message until the task is done
                while (!taskRunning.IsCompleted)
                {
                    Console.Write($"\rGame loading {spinner[counter % spinner.Length]}");

                    counter++;

                    // delays to make the spinner smooth
                    await Task.Delay(100); 
                }

                // clears the spinner line
                Console.Write("\rLoading... Done!                       ");

                // returns the result
                return await taskRunning;
            }
            catch (Exception ex)
            {
                // logs the exception
                _logger.LogError(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Prints the outcome of the shots
        /// </summary>
        private void ShotOutcome()
        {
            // represents shot outcomes: Miss = M, Hit = H, Sunk = S

            Console.Write("Miss: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("M");
            Console.ResetColor();
            Console.Write(" | ");

            Console.Write("Hit: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("H");
            Console.ResetColor();
            Console.Write(" | ");

            Console.Write("Sunk: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("S");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine();
        }

        /// <summary>
        /// Handles the exit process of the application
        /// </summary>
        private async Task Exit()
        {
            Console.WriteLine("Exiting the game...");
            await Task.Delay(1000);
            Environment.Exit(0);
        }

        /// <summary>
        /// Resets the game board
        /// </summary>
        private void ResetBoard()
        {
            Console.Clear();
            PrintHeader();
        }

        /// <summary>
        /// Prints an error on the game board
        /// </summary>
        /// <param name="message">The error message</param>
        private void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(message ?? "Error message is missing.");
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
