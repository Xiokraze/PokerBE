# Five-Card Stud Poker – Backend API

This is the backend API for the Five-Card Stud Poker take-home project, built with C# and ASP.NET Core. It provides core poker game logic, hand evaluation, and a RESTful endpoint to simulate a round of Five-Card Stud poker with multiple players.

## Features

- ♠️ Core domain models for cards, deck, player hands, and hand rankings
- 🃏 Robust poker hand evaluation according to standard 5-card poker rules
- 🎲 Game orchestration: deck shuffling, dealing, scoring, and winner determination
- 🚀 ASP.NET Core API endpoint (`/api/play/fiveCardStud`) accepting player names and returning detailed results
- 🔄 Supports ties and returns all top players in a format friendly for front-end consumption
- 🧩 Separation of concerns with DTOs for clean API responses
- 🔒 Basic error handling and validation

## Technologies Used

- C# with ASP.NET Core Web API
- Clean architecture principles for domain and API layers
- JSON serialization for lightweight payloads

## Setup

1. Ensure you have .NET SDK installed (version 6 or above recommended)
2. Run the API locally using:

```bash
dotnet run
```

The API will be hosted at http://localhost:5285 by default.

## API Usage

POST /api/play/fiveCardStud

Body example:

json
Copy
Edit
{
"playerNames": ["Alice", "Bob", "Carol"]
}
Response includes player hands, ranks, winner(s), and descriptive summaries.

## Project Structure Highlights

- Domain/: Core poker entities and game logic
- Services/: Poker hand evaluator and game orchestration services
- Controllers/: API endpoint definitions and request handling
- DTOs/: Data transfer objects for API communication

## Future Enhancements

- Support for additional poker variants (Texas Hold’em, Omaha, etc.)
- “Best-of-N” hand evaluation logic for games with more than 5 cards
- Comprehensive unit and integration tests
- Stateful game sessions with betting rounds and player turns
- API security features including authentication and rate limiting

## Related Project

This backend works together with the Five-Card Stud Poker Frontend Repo: https://github.com/Xiokraze/PokerFE
