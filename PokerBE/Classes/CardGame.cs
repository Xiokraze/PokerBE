namespace PokerBE.Classes
{
  /*
    Abstract base class for card games. Encapsulates shared logic
    like player tracking and hand management. Game-specific logic,
    such as dealing and scoring, is implemented in subclasses.

    Promotes reusability and enforces a common interface for
    all card game implementations.

    Returns game results from Deal().
  */
  public abstract class CardGame
  {
    public List<string> Players
    {
      get; protected set;
    }
    public List<List<Card>> Hands
    {
      get; protected set;
    }

    protected CardGame(List<string> playerNames)
    {
      Players = playerNames;

      // Ensure there are at least two players by adding a CPU if needed
      if (Players.Count == 1)
      {
        Players.Add("CPU");
      }
      Hands = new List<List<Card>>();
    }

    // Change return type to DealResult to allow returning game results
    public abstract DealResult Deal();
  }
}
