using PokerBE.Classes;

namespace PokerBE.Services
{
  public class GameSummary
  {
    /*
      Generates a human-readable summary string describing the evaluated poker hand.

      Examples:
        - "a Full House (Kings over Tens)"
        - "a Straight (10 to Ace)"
        - "a Pair of Jacks"

      Parameters:
        handRank - The evaluated HandRank containing the hand type and relevant card ranks.

      Returns:
        A descriptive string summarizing the poker hand for display or reporting.
    */
    public static string GetHandSummary(HandRank handRank)
    {
      if (handRank == null)
        return string.Empty;

      static string RankToString(int rank) => rank switch
      {
        14 => "Ace",
        13 => "King",
        12 => "Queen",
        11 => "Jack",
        10 => "Ten",
        9 => "Nine",
        8 => "Eight",
        7 => "Seven",
        6 => "Six",
        5 => "Five",
        4 => "Four",
        3 => "Three",
        2 => "Two",
        _ => rank.ToString()
      };

      switch (handRank.RankName)
      {
        case "Royal Flush":
          return "Royal Flush (Ace high)";
        case "Straight Flush":
          return $"Straight Flush to {RankToString(handRank.TieBreaker[0])}";
        case "Four of a Kind":
          return $"Four of a Kind: {RankToString(handRank.TieBreaker[0])}s";
        case "Full House":
          return $"Full House: {RankToString(handRank.TieBreaker[0])}s over {RankToString(handRank.TieBreaker[1])}s";
        case "Flush":
          return $"Flush, {RankToString(handRank.TieBreaker[0])} High";
        case "Straight":
          return $"Straight to {RankToString(handRank.TieBreaker[0])}";
        case "Three of a Kind":
          return $"Three of a Kind: {RankToString(handRank.TieBreaker[0])}s";
        case "Two Pair":
          return $"Two Pair: {RankToString(handRank.TieBreaker[0])}s and {RankToString(handRank.TieBreaker[1])}s";
        case "One Pair":
          return $"One Pair: {RankToString(handRank.TieBreaker[0])}s";
        case "High Card":
          return $"High Card: {RankToString(handRank.TieBreaker[0])}";
        default:
          return handRank.RankName;
      }
    }

    /*
      Constructs a summary string announcing the winner(s) of the poker game
      along with their winning hand description.

      Supports multiple winners by listing all player names separated by commas.
      If no winners or no winning hand is provided, returns "No winner".

      Parameters:
        winnerNames - List of winner player names.
        winningHand - The HandRank object representing the winning hand.

      Returns:
        A string summarizing the winner(s) and their winning hand.
    */
    public static string GetWinnerSummary(List<string> winnerNames, HandRank? winningHand)
    {
      if (winnerNames == null || winnerNames.Count == 0 || winningHand == null)
        return "No winner";

      var names = winnerNames.Count == 1
          ? winnerNames[0]
          : string.Join(", ", winnerNames);

      var summary = GetHandSummary(winningHand);

      return $"Winner{(winnerNames.Count > 1 ? "s" : "")}: {names} with {summary}";
    }
  }
}