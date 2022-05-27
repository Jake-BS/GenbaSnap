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
        public List<Player> PlayerList { get; set; }
        public List<Card> Deck { get; set; }

        public Table(int nOfPlayers)
        {
            NumberOfPlayers = nOfPlayers;
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

        public void StartRound()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            List<Card> curRoundCards = new List<Card>();
            foreach (var player in PlayerList)
            {
                
            }
        }
    }
}
