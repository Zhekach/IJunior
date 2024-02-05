using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Functions.OOP.Casino
{
    internal class Casino
    {
        static void Main()
        {
            DealerDeck dealerDeck = new DealerDeck();

            PlayerDeck playerDeck = new PlayerDeck(dealerDeck);

            bool isUserExited = false;

            while (isUserExited == false)
            {
                Console.Clear();

                PrintUI();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.TakeOneCard:
                        playerDeck.TakeOneCard();
                        break;
                    case (int)UserCommands.TakeSeveralCards:
                        TakeSeveralCardsUI(playerDeck);
                        break;
                    case (int)UserCommands.PrintMyCards:
                        playerDeck.PrintInfo();
                        break;
                    case (int)UserCommands.Exit:
                        isUserExited = true;
                        break;
                    default:
                        Console.WriteLine("You entered the wrong command. Try again, please.");
                        break;
                }

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }

        }

        static void PrintUI()
        {
            Console.WriteLine("Enter command:\n" +
                $"{(int)UserCommands.TakeOneCard} - if you need one more card\n" +
                $"{(int)UserCommands.TakeSeveralCards} - if you need to take several cards\n" +
                $"{(int)UserCommands.PrintMyCards} - print your cards info\n" +
                $"{(int)UserCommands.Exit} - exit\n");
        }

        static int ReadInt()
        {
            bool isIntEntered = false;
            int parsedInt = 0;

            while (isIntEntered == false)
            {
                string enteredString;

                Console.WriteLine("Enter integer:");
                enteredString = Console.ReadLine();

                isIntEntered = int.TryParse(enteredString, out parsedInt);

                if (isIntEntered)
                {
                    Console.WriteLine($"The entered number is recognized, it is: {parsedInt}");
                }
                else
                {
                    Console.WriteLine("You entered the wrong number. Try again, please.\n");
                }
            }

            return parsedInt;
        }

        static void TakeSeveralCardsUI(PlayerDeck playerDeck)
        {
            int cardsCount;

            Console.WriteLine("Enter the number of cards you need");

            cardsCount = ReadInt();
            playerDeck.TakeSeveralCards(cardsCount);
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
            foreach (Card card in cards)
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
            if (cards.Count > 0)
            {
                Card card;
                Random random = new Random();
                int cardsInDeck = cards.Count;
                int cardIndex = random.Next(0, cardsInDeck);

                card = cards[cardIndex];
                cards.RemoveAt(cardIndex);
                return card;
            }
            else
            {
                return null;
            }
        }
    }

    class PlayerDeck : DeckOfCards
    {
        private DealerDeck _dealerDeck;

        public PlayerDeck(DealerDeck dealer)
        {
            if (_dealerDeck != null)
            {
                _dealerDeck = dealer;
            }
            else
            {
                Console.WriteLine("It looks like you have no dealer =(");
            } 
        }

        public void TakeOneCard()
        {
            Card nextCard = null;

            if (_dealerDeck != null)
            {
                nextCard = _dealerDeck.DealCard();
            }
            else
            {
                Console.WriteLine("It looks like you have no more dealer =(");
            }

            if (nextCard != null)
            {
                cards.Add(nextCard);

                Console.WriteLine("You took card:");
                nextCard.PrintInfo();
            }
            else
            {
                Console.WriteLine("You couldn't take a card. It looks like the dealer has run out of them =(");
            }
        }

        public void TakeSeveralCards(int cardsCount)
        {
            if (cardsCount >= 1)
            {
                for (int i = 0; i < cardsCount; i++)
                {
                    TakeOneCard();
                    Console.WriteLine("Wait a second");
                    System.Threading.Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("It is necessary to enter a positive integer number of cards.");
            }
        }
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

    enum UserCommands
    {
        TakeOneCard = 1,
        TakeSeveralCards = 2,
        PrintMyCards = 3,
        Exit = 4,
    }
}
