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
            Console.WriteLine("Each time a card is revealed you will be given an opportunity to hit your key if your see a pair.");
            Console.WriteLine("Player 0's key is Q, Player 1's key is P, Player 2's key is Z, and Player 3's key is M.");
            Console.WriteLine("Press Enter after everyone has pressed their key, and the quickest will win the cards from the pairs' piles.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            var curFaceUpCards = new List<Card>();
            for (int i = 0; i < PlayerList.Count; i++)
            {
                curFaceUpCards.Add(new Card("None", "None"));
            }
            var curPlayerIndex = 0;
            var gameOver = false;
            List<string>? winners = null;
            int outOfCardsCounter = 0;
            while (!gameOver)
            {
                var player = PlayerList[curPlayerIndex];
                if (player.Pile.Count > 0)
                {
                    var revealedCard = player.Pile[player.Pile.Count - 1];
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
                        if (winningCard != null && winningCard.Rank != "None")
                        {
                            string winnerInput = snapInput.Substring(0, 1);
                            int winnerIndex = KeyboardPlayerDict[winnerInput];
                            Console.WriteLine("Player " + PlayerList[winnerIndex].Name + " wins the piles with " + winningCard.Rank + " on top!");
                            List<int> pairCardHolderIndexes = new List<int>();
                            int tempIndex = 0;
                            foreach (var card in curFaceUpCards)
                            {
                                if (card.Rank == winningCard.Rank) pairCardHolderIndexes.Add(tempIndex);
                                tempIndex++;
                            }
                            //Winner takes pair holders' face up cards
                            foreach (int pairIndex in pairCardHolderIndexes)
                            {
                                var totalPile = PlayerList[pairIndex].FaceUpPile;
                                totalPile.AddRange(PlayerList[winnerIndex].Pile);
                                PlayerList[winnerIndex].Pile = totalPile;
                                PlayerList[pairIndex].FaceUpPile = new List<Card>();
                            }
                            if (PlayerList[winnerIndex].Pile.Count == 52)
                            {
                                gameOver = true;
                                winners.Add(PlayerList[winnerIndex].Name);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Player " + player.Name + " is out of face down cards");
                    outOfCardsCounter++;
                    if (outOfCardsCounter == PlayerList.Count)
                    {
                        Console.WriteLine("All players are out of face down cards");
                        int highestCardCount = 0;
                        winners = new List<string>();
                        foreach (var tempPlayer in PlayerList)
                        {
                            if (tempPlayer.FaceUpPile.Count > highestCardCount)
                            {
                                highestCardCount = tempPlayer.FaceUpPile.Count;
                                winners = new List<string>();
                                winners.Add(tempPlayer.Name);
                            } else if (tempPlayer.FaceUpPile.Count == winners.Count) 
                                winners.Add(tempPlayer.Name);
                        }
                        gameOver = true;

                    }
                }
                
                
                curPlayerIndex++;
                if (curPlayerIndex >= PlayerList.Count) curPlayerIndex = 0;
                if (winners == null) 
                {
                    Console.WriteLine("Press enter to reveal next Card...");
                    Console.ReadLine();
                }
            }
            if (winners.Count == 1) Console.WriteLine("The winner is " + winners[0] + "!");
            else
            {
                Console.Write("The winners are ");
                foreach (var winner in winners)
                {
                    if (int.Parse(winner) != winners.Count - 1) Console.Write(winner + ", ");
                    else Console.WriteLine("and " + winner + "!");
                }
            }
        }
    }
}
