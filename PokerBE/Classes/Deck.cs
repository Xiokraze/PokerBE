namespace PokerBE.Classes
{
  public class Deck
  {
    /*
      Private field holding the current state of the deck.
      Declared at the class level so its state persists across method calls
      (e.g., Initialize, Shuffle, Deal). This allows the deck to be modified
      and reused throughout the lifecycle of a game session.
    */
    private List<Card> _cards;

    /*
      Represents a standard 52-card deck used in traditional card games.
      Provides functionality to initialize the deck, shuffle it fairly,
      and deal cards while ensuring no duplicates.

      This class is game-agnostic — it doesn't manage players or game rules,
      allowing it to be reused across various card game implementations.
    */
    public Deck()
    {
      _cards = new List<Card>();
      Initialize();
      Shuffle();
    }

    /*
      Fills the deck with a full set of 52 standard playing cards.
      Iterates over all combinations of Suit and Rank, creating one
      unique Card for each. Clears any existing cards to ensure a
      fresh deck.
    */
    private void Initialize()
    {
      _cards.Clear();

      /*
        The generic overload Enum.GetValues<T>() is used for type safety
        and avoids casting. It's more concise and eliminates boxing,
        improving clarity and performance.
      */
      foreach (Suit suit in Enum.GetValues<Suit>())
      {
        foreach (Rank rank in Enum.GetValues<Rank>())
        {
          _cards.Add(new Card(suit, rank));
        }
      }
    }

    /*
      Randomly shuffles the deck using the Fisher-Yates algorithm,
      ensuring each possible permutation is equally likely. This provides
      an unbiased and unpredictable shuffle, suitable for fair gameplay.

      Uses tuple deconstruction for concise in-place swapping.
    */
    public void Shuffle()
    {
      Random rng = new();
      int n = _cards.Count;
      for (int i = n - 1; i > 0; i--)
      {
        int j = rng.Next(i + 1);
        (_cards[j], _cards[i]) = (_cards[i], _cards[j]);
      }
    }

    /*
      Deals a specified number of cards from the top of the deck.
      Throws an exception if not enough cards remain.
      Removes and returns the dealt cards as a list.
    */
    public List<Card> Deal(int count)
    {
      if (count > _cards.Count)
      {
        throw new InvalidOperationException($"Cannot deal {count} cards. Only {_cards.Count} remaining.");
      }

      List<Card> hand = _cards.Take(count).ToList();
      _cards.RemoveRange(0, count);
      return hand;
    }

  }

}
