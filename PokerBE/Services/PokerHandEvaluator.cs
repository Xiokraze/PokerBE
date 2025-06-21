using PokerBE.Classes;

namespace PokerBE.Services
{
  /*
    PokerHandEvaluator

    Provides static methods to evaluate a poker hand and return its strength
    as a HandRank object. Designed to support flexible hand sizes and is
    easily extensible for different poker game variants.

    Key Responsibilities:
    - Detect and score all standard poker hands: High Card through Royal Flush.
    - Use tie-breaker logic for determining winners when hands share the same type.
    - Maintain clean separation of evaluation logic from game mechanics.

    Design Decisions:
    - Aces are treated as high only (no low-Ace straights like A-2-3-4-5).
    - Evaluation only considers the first 'handSize' cards for now.
    - Future versions could support best-N-of-M hand analysis for games like Texas Hold'em.

    Usage:
    Call EvaluateHandRank(List<Card> hand, int handSize) to analyze a hand and
    receive a comparable HandRank result.
  */

  public class PokerHandEvaluator
  {
    /*
      Checks if all cards share the same suit.
      Used for identifying flush-based hands like Flush, Straight Flush, Royal Flush.
    */
    private static bool IsFlush(List<Card> cards)
    {
      return cards.All(c => c.Suit == cards[0].Suit);
    }

    /*
      Determines if the hand is a sequence of consecutive ranks.
      Note: Aces are only treated as high in this project, not as low (no A-2-3-4-5).
    */
    private static bool IsStraight(List<Card> cards)
    {
      var orderedRanks = cards
          .Select(c => (int)c.Rank)
          .OrderBy(r => r)
          .ToList();

      for (int i = 1; i < orderedRanks.Count; i++)
      {
        if (orderedRanks[i] != orderedRanks[i - 1] + 1)
          return false;
      }

      return true;
    }

    /*
      Groups cards by rank to identify patterns like pairs, trips, quads, etc.
      Returns a dictionary: key = rank value, value = count of occurrences.
    */
    private static Dictionary<int, int> GroupByRank(List<Card> cards)
    {
      return cards
          .GroupBy(c => (int)c.Rank)
          .ToDictionary(g => g.Key, g => g.Count());
    }

    /*
      Main evaluator that assigns a HandRank to a hand of cards.

      Parameters:
      - hand: List of cards dealt to the player.
      - handSize: Number of cards to actually consider when evaluating.

      Why it's designed this way:
      - Supports future scalability (e.g., 7-card stud, best-of-N evaluation).
      - Logic is encapsulated per hand type, so future variants (e.g. Jokers, wildcards)
        could be added by changing this method or extending the class.

      Tradeoff: For now, we evaluate only the first `handSize` cards, assuming
      they were pre-selected or the game is 5-card stud (like this project).
    */
    public static HandRank EvaluateHandRank(List<Card> hand, int handSize)
    {
      // Guard. Hand is null.
      ArgumentNullException.ThrowIfNull(hand);

      // Guard. Hand count is less than size.
      if (hand.Count < handSize)
        throw new ArgumentException($"Hand must contain at least {handSize} cards.");

      // Truncate to expected hand size. Future version could compute best N-of-X.
      var cardsToEvaluate = hand.Take(handSize).ToList();

      // Precomputed values reused throughout hand evaluation
      bool flush = IsFlush(cardsToEvaluate);
      bool straight = IsStraight(cardsToEvaluate);
      var rankGroups = GroupByRank(cardsToEvaluate);
      var orderedGroups = rankGroups.OrderByDescending(g => g.Value).ThenByDescending(g => g.Key).ToList();

      // STRAIGHT FLUSH / ROYAL FLUSH
      if (flush && straight)
      {
        bool isRoyal = cardsToEvaluate.Any(c => c.Rank == Rank.Ace) && cardsToEvaluate.Min(c => (int)c.Rank) == 10;
        if (isRoyal)
          return new HandRank("Royal Flush", 10, new List<int> { (int)Rank.Ace }, cardsToEvaluate);
        else
          return new HandRank("Straight Flush", 9, new List<int> { cardsToEvaluate.Max(c => (int)c.Rank) }, cardsToEvaluate);
      }

      // FOUR OF A KIND
      if (orderedGroups[0].Value == 4)
      {
        int fourKindRank = orderedGroups[0].Key;
        int kicker = orderedGroups[1].Key;
        return new HandRank("Four of a Kind", 8, new List<int> { fourKindRank, kicker }, cardsToEvaluate);
      }

      // FULL HOUSE
      if (orderedGroups[0].Value == 3 && orderedGroups[1].Value == 2)
      {
        int threeKindRank = orderedGroups[0].Key;
        int pairRank = orderedGroups[1].Key;
        return new HandRank("Full House", 7, new List<int> { threeKindRank, pairRank }, cardsToEvaluate);
      }

      // FLUSH
      if (flush)
      {
        var ranksDescending = cardsToEvaluate.Select(c => (int)c.Rank).OrderByDescending(r => r).ToList();
        return new HandRank("Flush", 6, ranksDescending, cardsToEvaluate);
      }

      // STRAIGHT
      if (straight)
      {
        return new HandRank("Straight", 5, new List<int> { cardsToEvaluate.Max(c => (int)c.Rank) }, cardsToEvaluate);
      }

      // THREE OF A KIND
      if (orderedGroups[0].Value == 3)
      {
        int threeKindRank = orderedGroups[0].Key;
        var kickers = orderedGroups.Skip(1).Select(g => g.Key).OrderByDescending(k => k).ToList();
        var tieBreaker = new List<int> { threeKindRank };
        tieBreaker.AddRange(kickers);
        return new HandRank("Three of a Kind", 4, tieBreaker, cardsToEvaluate);
      }

      // TWO PAIR
      if (orderedGroups[0].Value == 2 && orderedGroups[1].Value == 2)
      {
        var pairs = orderedGroups.Take(2).Select(g => g.Key).OrderByDescending(k => k).ToList();
        var kicker = orderedGroups[2].Key;
        var tieBreaker = new List<int>(pairs) { kicker };
        return new HandRank("Two Pair", 3, tieBreaker, cardsToEvaluate);
      }

      // ONE PAIR
      if (orderedGroups[0].Value == 2)
      {
        int pairRank = orderedGroups[0].Key;
        var kickers = orderedGroups.Skip(1).Select(g => g.Key).OrderByDescending(k => k).ToList();
        var tieBreaker = new List<int> { pairRank };
        tieBreaker.AddRange(kickers);
        return new HandRank("One Pair", 2, tieBreaker, cardsToEvaluate);
      }

      // HIGH CARD
      var sortedRanks = cardsToEvaluate.Select(c => (int)c.Rank).OrderByDescending(r => r).ToList();
      return new HandRank("High Card", 1, sortedRanks, cardsToEvaluate);
    }

  }
}
