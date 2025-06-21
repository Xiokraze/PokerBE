/*
 Represents the four standard suits in a 52-card deck.
 Using an enum enforces valid values and makes logic like
 sorting and comparison easier and safer than strings.
*/
public enum Suit { Clubs, Diamonds, Hearts, Spades }

/*
 Represents the ranks from 2 to Ace.
 Assigning numeric values allows for easy comparison between
 cards in game logic (e.g., determining high card). Enum makes
 the code safer and more readable than raw strings or integers.
*/
public enum Rank
{
    Two = 2, Three, Four, Five, Six, Seven, Eight, Nine, Ten,
    Jack, Queen, King, Ace
}

/*
 Represents a single playing card composed of a Suit and a Rank.
 Uses 'init' accessors to make the object immutable after creation,
 aligning with the real-world behavior of cards, which do not change once dealt.
 This improves safety, simplifies debugging, and allows use in read-only contexts.
*/
public class Card
{
    public Suit Suit { get; init; }
    public Rank Rank { get; init; }

    public override string ToString()
    {
        return $"{Rank} of {Suit}";

    }
}

// test
