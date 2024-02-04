using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.OOP.Casino
{
    internal class Casino
    {
        static void Main()
        {


            DealerDeck dealerDeck = new DealerDeck();
            dealerDeck.PrintInfo();
        }
    }

    abstract class DeckOfCards
    {
        protected List<Card> cards;

        protected DeckOfCards() 
        {
            cards = new List<Card>();
        }

        public void PrintInfo()
        {
            foreach(Card card in cards)
            {
                card.PrintInfo();
            }
        }
    }

    class DealerDeck : DeckOfCards
    {
        public DealerDeck() 
        {
            foreach (Values value in Enum.GetValues(typeof(Values)))
            {
                foreach (Suits suits in Enum.GetValues(typeof(Suits)))
                {
                    Card card = new Card((Suits)suits, (Values)value);
                    cards.Add(card);
                }
            }
        }

        public Card DealCard()
        {
            Card card = null;
            return card;
        }
    }

    class PlayerDeck : DeckOfCards
    {
        DealerDeck _dealer;

        public PlayerDeck(DealerDeck dealer) 
        {
            _dealer = dealer;
        }

        public void GetDealerCard()
        {
            cards.Add(_dealer.DealCard());
        }

        public void 
    }

    class Card
    {
        public Suits Suit { get; private set; }
        public Values Value { get; private set; }

        public Card(Suits suit, Values value)
        {
            Suit = suit;
            Value = value;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Card is {Value} of {Suit}");
        }
    }

    enum Suits
    {
        Spades,
        Hearts,
        Clubs,
        Diamonds
    }

    enum Values
    {
        Ace = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13
    }
}
