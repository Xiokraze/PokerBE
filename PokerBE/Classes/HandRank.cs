namespace PokerBE.Classes
{
  /*
      Represents the evaluated rank of a poker hand.

      Responsibilities:
      - Store the hand type (e.g., "One Pair", "Flush") as a human-readable name.
      - Store a numeric RankValue to allow quick comparison of hand strengths.
        Higher values indicate stronger hands.
      - Maintain a list of tie-breaker values representing relevant card ranks
        ordered from highest to lowest. This enables detailed comparison when
        two hands have the same RankValue (e.g., both have One Pair, but with
        different pair ranks or kickers).
      - Store the actual cards that form the evaluated hand for
        display or debugging purposes.

      Implements IComparable to provide natural ordering between HandRank instances,
      allowing easy sorting and winner determination based on poker rules.
    */
  public class HandRank : IComparable<HandRank>
  {
    public string RankName
    {
      get; init;
    }
    public int RankValue
    {
      get; init;
    }
    /*
      TieBreaker stores a list of integer card ranks relevant for breaking ties
      between hands of the same RankValue. For example:
        - For One Pair: first element might be the rank of the pair,
          followed by kickers in descending order.
        - For Full House: first element could be rank of the triplet,
          second element rank of the pair.
      This ordered list is used to compare two equal-ranked hands in detail.
    */
    public List<int> TieBreaker { get; init; } = new(); // High-to-low card ranks for tie-breaking
    public List<Card> HandCards { get; init; } = new();

    /*
      Constructor initializes the HandRank with all relevant data.
      Null coalescing for tieBreaker and handCards allows callers
      to omit them when not needed.
    */
    public HandRank(string rankName, int rankValue, List<int>? tieBreaker = null, List<Card>? handCards = null)
    {
      RankName = rankName;
      RankValue = rankValue;
      if (tieBreaker != null)
        TieBreaker = tieBreaker;
      if (handCards != null)
        HandCards = handCards;
    }

    /*
      Compares this HandRank instance to another to establish ordering.

      Logic:
        1. Compare the RankValue (hand type strength) directly.
             If they differ, this determines the winner.
        2. If RankValues are equal, compare TieBreaker values in order.
             Iterate through tie-breaker list elements and return on first difference.
        3. If all tie-breakers are equal or missing, hands are considered equal.

      This supports poker’s standard tie-breaking rules naturally and efficiently.

      Example scenario for tie-breaking logic:

      Two players both have "Two Pair" hands:
      - Player 1: Pair of Kings, Pair of 3s, kicker 8
      - Player 2: Pair of Kings, Pair of 3s, kicker 5

      HandRank stores:
      - RankValue representing "Two Pair"
      - TieBreaker list containing [King (13), 3, kicker]

      Comparison steps:
      1. Compare RankValue (both equal → continue)
      2. Compare highest pair rank (13 vs 13 → tie)
      3. Compare second pair rank (3 vs 3 → tie)
      4. Compare kicker (8 vs 5 → Player 1 wins)

      This ordered tie-breaker list allows precise and correct
      evaluation of poker hands with identical rank types.
    */
    public int CompareTo(HandRank? other)
    {
      if (other == null)
        return 1;

      int cmp = RankValue.CompareTo(other.RankValue);
      if (cmp != 0)
        return cmp;

      int minCount = Math.Min(TieBreaker.Count, other.TieBreaker.Count);
      for (int i = 0; i < minCount; i++)
      {
        cmp = TieBreaker[i].CompareTo(other.TieBreaker[i]);
        if (cmp != 0)
          return cmp;
      }

      return 0; // hands are equal
    }
  }
}
