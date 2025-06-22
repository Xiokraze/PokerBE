namespace PokerBE.DTOs
{
  /*
    GameResultDto

    A simplified version of DealResult used for returning
    game outcomes to the frontend.

    Includes:
    - PlayerResults: list of each player's name, card strings, and hand rank.
    - Winners: list of winner names.
    - Reason: summary reason like "Flush" or "Two Pair".
  */
  public class GameResultDto
  {
    public List<PlayerResultDto> PlayerResults { get; set; } = new();
    public List<string> Winners { get; set; } = new();
    public string Reason { get; set; } = string.Empty;
  }
}
