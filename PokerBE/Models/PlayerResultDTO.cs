namespace PokerBE.DTOs
{
  /*
    PlayerResultDto

    Data Transfer Object used to represent a simplified view of a player's hand
    and outcome after a game of poker.

    Purpose:
    - Decouples internal model complexity (e.g. Card, HandRank, etc.) from the API response.
    - Keeps API responses lightweight, frontend-friendly, and focused on display needs.
    - Prevents unintentional exposure of backend logic or extra data.
    - Supports consistent formatting (e.g., converting cards into short strings like "KD", "9H").

    Example Output:
    {
      "player": "Alice",
      "cards": ["KD", "9H", "7S", "6D", "3C"],
      "rank": "One Pair"
    }

    This DTO is returned by the controller instead of raw models like PlayerHand,
    reducing payload size and simplifying rendering on the client side.
  */
  public class PlayerResultDto
  {
    public string Player { get; set; } = string.Empty;

    // List of cards as short strings, e.g., "AH", "10S", "2D"
    public List<string> Cards { get; set; } = new();

    // Human-readable name of the hand rank, e.g., "Flush", "Full House"
    public string Rank { get; set; } = string.Empty;

    // Readable hand description e.g. "One Pair: Sixes with King High"
    public string HandSummary { get; set; } = string.Empty;
  }
}
