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
        public List<string>? Winners { get; set; }

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
            Winners = null;
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
            IntroText();
            var curFaceUpCards = new List<Card>();
            for (int i = 0; i < PlayerList.Count; i++)
            {
                curFaceUpCards.Add(new Card("None", "None"));
            }
            var curPlayerIndex = 0;
            var gameOver = false;
            while (!gameOver)
            {
                var player = PlayerList[curPlayerIndex];
                if (player.Pile.Count > 0)
                {
                    PlayerHasPile(player, curFaceUpCards, gameOver);
                }
                else
                {
                    Console.WriteLine("Player " + player.Name + " is out of face down cards");
                    int outOfCardsCounter = 0;
                    foreach (var playerToTestEmpty in PlayerList) if(playerToTestEmpty.Pile.Count == 0) outOfCardsCounter++;
                    if (outOfCardsCounter == PlayerList.Count)
                    {
                        GameDoneFindWinner();
                        gameOver = true;
                    }
                }
                
                curPlayerIndex++;
                if (curPlayerIndex >= PlayerList.Count) curPlayerIndex = 0;
                if (Winners == null) 
                {
                    Console.WriteLine("Press enter to reveal next Card...");
                    Console.ReadLine();
                }
            }
            Console.WriteLine(OutroText());
        }

        public void GameDoneFindWinner()
        {
            Console.WriteLine("All players are out of face down cards");
            int highestCardCount = 0;
            Winners = new List<string>();
            foreach (var tempPlayer in PlayerList)
            {
                if (tempPlayer.FaceUpPile.Count > highestCardCount)
                {
                    highestCardCount = tempPlayer.FaceUpPile.Count;
                    Winners = new List<string>();
                    Winners.Add(tempPlayer.Name);
                }
                else if (tempPlayer.FaceUpPile.Count == highestCardCount)
                {
                    highestCardCount = tempPlayer.FaceUpPile.Count;
                    Winners.Add(tempPlayer.Name);
                }
            }
        }

        public string OutroText()
        {
            string outroText = "";
            if (Winners.Count == 1) outroText = "The winner is Player " + Winners[0] + "!";
            else
            {
                outroText = "The winners are Players ";
                foreach (var winner in Winners)
                {
                    if (int.Parse(winner) != Winners.Count - 1) outroText += winner + ", ";
                    else outroText += "and " + winner + "!";
                }
            }
            return outroText;
        }

        private bool PlayerHasPile(Player player, List<Card> curFaceUpCards, bool gameOver)
        {
            var revealedCard = player.Pile[player.Pile.Count - 1];
            Console.WriteLine("Player " + player.Name + "'s card is: " + revealedCard.Name + ".");
            player.FaceUpPile.Add(revealedCard);
            player.Pile.RemoveAt(player.Pile.Count - 1);
            curFaceUpCards[int.Parse(player.Name)] = revealedCard;
            Console.WriteLine("Press your key if you see a pair!");
            var snapInput = Console.ReadLine();
            snapInput = ValidateInput(snapInput);
            if (snapInput != "")
            {
                Card? winningCard = null;
                //Searching current face up cards to see if snap is valid
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
                    gameOver = SuccessfulSnap(gameOver, snapInput, winningCard, curFaceUpCards);
                }
            }
            return gameOver;
        }

        public bool SuccessfulSnap(bool gameOver, string snapInput, Card winningCard, List<Card> curFaceUpCards)
        {
            int winnerIndex = KeyboardPlayerDict[snapInput];
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
                Winners = new List<string>();
                Winners.Add(PlayerList[winnerIndex].Name);
            }
            return gameOver;
        }

        private void IntroText()
        {
            Console.WriteLine("Each time a card is revealed you will be given an opportunity to hit your key if your see a pair.");
            Console.WriteLine();
            Console.WriteLine("Player 0's key is Q, Player 1's key is P, Player 2's key is Z, and Player 3's key is M.");
            Console.WriteLine("Press Enter after everyone has pressed their key, and the quickest will win the cards from the pairs' piles.");
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        public string ValidateInput(string snapInput)
        {
            bool legitInput = false;
            //Stops caps lock from breaking inputs
            foreach (var letter in snapInput.ToLower())
            {
                if (KeyboardPlayerDict.ContainsKey(letter.ToString()))
                {
                    if (KeyboardPlayerDict[letter.ToString()] < PlayerList.Count)
                    {
                        legitInput = true;
                        snapInput = letter.ToString();
                        break;
                    }
                }
            }
            if (!legitInput) snapInput = "";
            return snapInput;
        }
    }
}
