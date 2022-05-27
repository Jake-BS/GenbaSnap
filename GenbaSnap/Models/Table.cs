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
            PlayerList = SeatPlayers();
        }

        private List<Card> CreateDeck()
        {
            var deck = new List<Card>();
            var ranks = new List<string>() {"ace", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "jack", "queen", "king"};
            var suits = new List<string>() { "clubs", "diamonds", "hearts", "spades" };
            foreach (var rank in ranks)
            {
                foreach (var suit in suits) deck.Add(new Card(rank, suit));
            }
            return deck;
        }

        private List<Player> SeatPlayers()
        {
            List<Player> playerList = new List<Player>();
            for (int i = 0; i < NumberOfPlayers; i++)
            {
                var curPlayer = new Player(i.ToString());
                playerList.Add(curPlayer);
            }
            Shuffle();
            //current player for dealing
            var CPFD = 0;
            foreach (var card in Deck)
            {
                playerList[CPFD].Pile.Add(card);
            }
            return playerList;
        }

        private void Shuffle()
        {
            //From https://www.delftstack.com/howto/csharp/shuffle-a-list-in-csharp/
            var rnd = new Random();
            var randomized = Deck.OrderBy(item => rnd.Next());
        }
    }
}
