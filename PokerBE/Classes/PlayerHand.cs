namespace PokerBE.Classes
{/*
  Represents an individual player's hand and its evaluated ranking.
*/
  public class PlayerHand
  {
    public string Player { get; set; } = string.Empty;
    public List<Card> Cards { get; set; } = new();
    public HandRank Rank { get; set; } = null!;
    // public List<string> DisplayCards => Cards.Select(c => c.ToDisplayString()).ToList();

  }
}
