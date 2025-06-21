namespace PokerBE.Classes
{
  /*
    Represents the result of a completed deal in a poker game.

    Includes:
    - All player hands and their evaluated ranks.
    - A list of winners (can be more than one in case of a tie).
    - A human-readable reason for the outcome (usually the name of the best hand).
  */
  public class DealResult
  {
    public List<PlayerHand> Hands { get; set; } = new();
    public List<string> Winners { get; set; } = new();
    public string Reason { get; set; } = string.Empty;
  }

}
