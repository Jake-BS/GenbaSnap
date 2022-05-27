using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenbaSnap.Models
{
    public class Card
    {
        public string Rank { get; set; }
        public string Suit { get; set; }

        public Card(string rank, string suit)
        {
            Rank = rank;
            Suit = suit;
        }
    }
}
