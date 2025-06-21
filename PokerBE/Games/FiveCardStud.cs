using PokerBE.Services;

namespace PokerBE.Classes
{
  /*
    Implementation of a five-card stud poker game.

    Responsibilities:
    - Manage the deck and deal 5 cards to each player.
    - Evaluate each player’s hand using PokerHandEvaluator.
    - Determine winner(s) and return structured game result.
    - Supports multiple rounds by resetting and shuffling the deck on each deal.
    - Validates that there are enough cards for all players.
  */
  public class FiveCardStud : CardGame
  {
    private const int HandSize = 5;
    private const int DeckSize = 52;
    private Deck _deck;

    public FiveCardStud(List<string> playerNames) : base(playerNames)
    {
      _deck = new Deck();
    }

    /*
      Deals 5 cards to each player, evaluates their hands,
      and returns the result including winner(s) and ranking rationale.

      Throws InvalidOperationException if not enough cards for all players.

      Handles ties by returning all players with the highest hand rank.
    */
    public DealResult Play()
    {
      _deck = new Deck();

      int totalCardsNeeded = Players.Count * HandSize;
      if (totalCardsNeeded > DeckSize)
      {
        throw new InvalidOperationException($"Not enough cards in the deck to deal {HandSize} cards to {Players.Count} players.");
      }

      Hands.Clear();

      foreach (var _ in Players)
      {
        var hand = _deck.Deal(HandSize);
        Hands.Add(hand);
      }

      var evaluated = new List<PlayerHand>();

      foreach (var (player, index) in Players.Select((p, i) => (p, i)))
      {
        try
        {
          var rank = PokerHandEvaluator.EvaluateHandRank(Hands[index], HandSize);
          evaluated.Add(new PlayerHand
          {
            Player = player,
            Cards = Hands[index],
            Rank = rank
          });
        }
        catch (Exception ex)
        {
          // Log or handle evaluation errors here if needed
          // For now, just throw
          throw new InvalidOperationException($"Error evaluating hand for player {player}: {ex.Message}", ex);
        }
      }

      if (evaluated.Count == 0)
      {
        return new DealResult
        {
          Hands = evaluated,
          Winners = new List<string>(),
          Reason = "No hands evaluated"
        };
      }

      // Safe to use '!' because evaluated is non-empty
      var highestRank = evaluated.Max(ph => ph.Rank)!;

      var winners = evaluated
        .Where(ph => ph.Rank.CompareTo(highestRank) == 0)
        .Select(ph => ph.Player)
        .ToList();

      return new DealResult
      {
        Hands = evaluated,
        Winners = winners,
        Reason = highestRank.RankName
      };
    }


    public override DealResult Deal()
    {
      return Play();
    }
  }
}
