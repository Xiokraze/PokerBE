namespace PokerBE.Classes
{
  /*
    Represents a request to start a poker game.
    Contains a list of player names that will be used
    to generate and evaluate hands on the backend.

    This is used as the request body in API calls
    like /api/games/five-card-stud.
  */
  public class PlayerRequest
  {
    public required List<string> PlayerNames
    {
      get; set;
    }
  }
}
