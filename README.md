# BattleshipAPI

BattleshipConsole is a console application that interacts with the BattleshipAPI. It consumes the RESTful API to implement the classic Battleship game logic. The application features random ship placement on a grid and allows players to fire shots to try and sink their opponent's ships. Built using .NET 6, the application follows a client-server architecture pattern, with the console app serving as the client communicating with the BattleshipAPI.

## Features

- Place ships of varying sizes on a 10x10 grid.
- Fire shots at a specific row and column to hit or miss enemy ships.
- Keep track of the shooting history.
- Detect when ships are sunk and the game is won.
- Handle invalid ship placements and invalid or same shot positions.

## Technologies Used

- **.NET 6**
- **C#**

## Getting Started

### Prerequisites

- .NET 6 SDK or later
- Visual Studio 2022 or VS Code with C# extension

### HTTP Header

- Add the custom 'X-consumer' HTTP header to each request. This header will be used to uniquely identify individual users and is essential for handling user-specific data caching processes.

## Support

Darshana Wijesinghe  
Email address - [dar.mail.work@gmail.com](mailto:dar.mail.work@gmail.com)  
Linkedin - [darwijesinghe](https://www.linkedin.com/in/darwijesinghe/)  
GitHub - [darwijesinghe](https://github.com/darwijesinghe)

## License

This project is licensed under the terms of the **MIT** license.
