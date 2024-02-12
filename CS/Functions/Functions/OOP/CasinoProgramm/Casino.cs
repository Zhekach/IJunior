using System;
using System.Collections.Generic;

namespace Functions.OOP.CasinoProgramm
{
    internal class CasinoProgramm
    {
        static void Main()
        {
            Casino casino = new Casino();
            casino.Run();
        }
    }

    class Casino
    {
        private bool _isUserExited = false;
        private Dealer _dealer;
        private Player _player;

        public Casino()
        {
            _player = new Player();
            DeckOfCards deckOfCards = new DeckOfCards();
            _dealer = new Dealer(_player, deckOfCards);
        }

        public void Run()
        {
            while (_isUserExited == false)
            {
                Console.Clear();

                PrintUI();

                int userInput = ReadInt();

                switch (userInput)
                {
                    case (int)UserCommands.TakeOneCard:
                        _dealer.TransferOneCard();
                        break;

                    case (int)UserCommands.TakeSeveralCards:
                        TakeSeveralCardsUI(_dealer);
                        break;

                    case (int)UserCommands.PrintMyCards:
                        _player.PrintCardsInfo();
                        break;

                    case (int)UserCommands.Exit:
                        _isUserExited = true;
                        break;

                    default:
                        Console.WriteLine("You entered the wrong command. Try again, please.");
                        break;
                }

                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        private void PrintUI()
        {
            Console.WriteLine("Enter command:\n" +
                $"{(int)UserCommands.TakeOneCard} - if you need one more card\n" +
                $"{(int)UserCommands.TakeSeveralCards} - if you need to take several cards\n" +
                $"{(int)UserCommands.PrintMyCards} - print your cards info\n" +
                $"{(int)UserCommands.Exit} - exit\n");
        }

        private int ReadInt()
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

        private void TakeSeveralCardsUI(Dealer dealer)
        {
            int cardsCount;

            Console.WriteLine("Enter the number of cards you need");

            cardsCount = ReadInt();
            dealer.TransferSeveralCards(cardsCount);
        }
    }

    class DeckOfCards
    {
        private List<Card> _cards;

        public DeckOfCards()
        {
            _cards = new List<Card>();

            Array values = Enum.GetValues(typeof(Values));
            Array suits = Enum.GetValues(typeof(Suits));

            foreach (Values value in values)
            {
                foreach (Suits suit in suits)
                {
                    Card card = new Card((Suits)suit, (Values)value);
                    _cards.Add(card);
                }
            }

            Shuffle();
        }

        public bool TryGiveCard(out Card card)
        {
            if (_cards.Count > 0)
            {
                card = _cards[0];
                _cards.RemoveAt(0);
                return true;
            }
            else
            {
                card = null;
                return false;
            }
        }

        public void Shuffle()
        {
            Random random = new Random();

            for (int i = _cards.Count - 1; i >= 1; i--)
            {
                int j = random.Next(_cards.Count);
                Card tempCard = _cards[j];
                _cards[j] = _cards[i];
                _cards[i] = tempCard;
            }
        }
    }

    class Dealer
    {
        private Player _player;
        private DeckOfCards _deckOfCards;

        public Dealer(Player player, DeckOfCards deckOfCards)
        {
            _player = player;
            _deckOfCards = deckOfCards;
        }

        public void TransferOneCard()
        {
            Card tempCard;

            if (_deckOfCards.TryGiveCard(out tempCard))
            {
                _player.TakeCard(tempCard);
            }
            else
            {
                Console.WriteLine("It looks like there are no more cards in the deck");
            }
        }

        public void TransferSeveralCards(int count)
        {
            if (count >= 1)
            {
                for (int i = 0; i < count; i++)
                {
                    TransferOneCard();
                }
            }
            else
            {
                Console.WriteLine("It is necessary to enter a positive integer number of cards.");
            }
        }
    }

    class Player
    {
        private List<Card> _cards;

        public Player()
        {
            _cards = new List<Card>();
        }

        public void TakeCard(Card card)
        {
            _cards.Add(card);

            Console.WriteLine("You took card:");
            card.PrintInfo();
        }

        public void PrintCardsInfo()
        {
            foreach (Card card in _cards)
            {
                card.PrintInfo();
            }
        }
    }

    class Card
    {
        private Suits _suit;
        private Values _value;

        public Card(Suits suit, Values value)
        {
            _suit = suit;
            _value = value;
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Card is {_value} of {_suit}");
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