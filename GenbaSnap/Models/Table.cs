using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenbaSnap.Models
{
    public class Table
    {
        public int NumberOfPlayers { get; set; }
        public Dictionary<string, int> KeyboardPlayerDict { get; set; }
        public List<Player> PlayerList { get; set; }
        public List<Card> Deck { get; set; }

        public Table(int nOfPlayers)
        {
            NumberOfPlayers = nOfPlayers;
            KeyboardPlayerDict = new Dictionary<string, int>() {
                { "q", 0},
                { "p", 1},
                { "z", 2},
                { "m", 3 }
            };
            Deck = CreateDeck();
            PlayerList = DealPlayers();
        }

        public List<Card> CreateDeck()
        {
            var deck = new List<Card>();
            var ranks = new List<string>() {"ace", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "jack", "queen", "king"};
            var suits = new List<string>() { "clubs", "diamonds", "hearts", "spades" };
            foreach (var rank in ranks)
            {
                foreach (var suit in suits) deck.Add(new Card(rank, suit));
            }
            return deck;
        }

        private List<Player> DealPlayers()
        {
            List<Player> playerList = new List<Player>();
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                var curPlayer = new Player(i.ToString());
                playerList.Add(curPlayer);
            }
            //randomizes the order of the cards in the Deck property
            Shuffle();
            //CDFD -> current player for dealing
            var CPFD = 0;
            foreach (var card in Deck)
            {
                playerList[CPFD].Pile.Add(card);
                CPFD++;
                if (CPFD >= NumberOfPlayers) CPFD = 0;
            }
            return playerList;
        }

        public void Shuffle()
        {
            //From https://www.delftstack.com/howto/csharp/shuffle-a-list-in-csharp/
            var rnd = new Random();
            var randomized = Deck.OrderBy(item => rnd.Next());
            Deck = new List<Card>();
            foreach (var card in randomized) Deck.Add(card);
        }

        public void Start()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            Card[] curFaceUpCards = new Card[PlayerList.Count];
            var curPlayerIndex = 0;
            var gameOver = false;
            string? winner = null;
            while (!gameOver)
            {
                var player = PlayerList[curPlayerIndex];
                var revealedCard = player.Pile[player.Pile.Count-1];
                Console.WriteLine("Player " + player.Name + "'s card is: " + revealedCard.Name + ".");
                player.FaceUpPile.Add(revealedCard);
                player.Pile.RemoveAt(player.Pile.Count - 1);
                curFaceUpCards[int.Parse(player.Name)] = revealedCard;
                Console.WriteLine("Press your key if you see a pair!");
                var snapInput = Console.ReadLine();
                if (snapInput != "")
                {
                    Card? winningCard = null;
                    foreach (var card in curFaceUpCards)
                    {
                        int cardCounter = 0;
                        foreach (var card2 in curFaceUpCards)
                        {
                            if (card.Rank == card2.Rank)
                            {
                                cardCounter++;
                                if (cardCounter > 1)
                                {
                                    winningCard = card2;
                                }
                            } 
                        }
                    }
                    if (winningCard != null)
                    {
                        string winnerInput = snapInput.Substring(0);
                        int winnerIndex = KeyboardPlayerDict[winnerInput];
                        Console.WriteLine("Player " + PlayerList[winnerIndex].Name + " wins the " + winningCard.Rank + " piles!");
                        List<int> pairCardHolderIndexes = new List<int>();
                        foreach (var card in curFaceUpCards)
                        {
                            if (card.Rank == winningCard.Rank) pairCardHolderIndexes.Add(Array.IndexOf(curFaceUpCards, card));
                        }
                        //Winner takes pair holders' face up cards
                        foreach (int pairIndex in pairCardHolderIndexes)
                        {
                            PlayerList[winnerIndex].Pile = (List<Card>)PlayerList[pairIndex].FaceUpPile.Concat(PlayerList[winnerIndex].Pile);
                            PlayerList[pairIndex].FaceUpPile = new List<Card>();
                        }
                        if (PlayerList[winnerIndex].Pile.Count == 52)
                        {
                            gameOver = true;
                            winner = PlayerList[winnerIndex].Name;
                        }
                    }
                }
                
                curPlayerIndex++;
                if (curPlayerIndex >= PlayerList.Count) curPlayerIndex = 0;
                if (winner == null) 
                {
                    Console.WriteLine("Press enter to reveal next Card...");
                    Console.ReadLine();
                }
            }
            Console.WriteLine("The winner is " + winner + "!");
        }
    }
}
