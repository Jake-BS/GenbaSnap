﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenbaSnap.Models
{
    public class Player
    {
        public string Name { get; set; }
        public List<Card> Pile {get; set;}

        public Player(string name)
        {
            Name = name;
            Pile = new List<Card>();
        }
    }
}
