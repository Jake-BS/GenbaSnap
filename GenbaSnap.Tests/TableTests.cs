using Autofac.Extras.Moq;
using GenbaSnap.Models;
using NUnit.Framework;
using System;

namespace GenbaSnap.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateDeckTest()
        {
            var testTable = new Table(3);
            var testDeck = testTable.CreateDeck();
            foreach (var card in testDeck) Console.WriteLine(card.Name);
            Assert.AreEqual(52, testDeck.Count);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void DealPlayersTests(int numberOfPlayers)
        {
            var testTable = new Table(numberOfPlayers);
            Console.WriteLine(testTable.PlayerList[0].Pile[0].Name);
            //Checks the correct number of players have been dealt to at the table
            Assert.AreEqual(numberOfPlayers, testTable.PlayerList.Count);
            //checks first and second players do not have the same cards (in one deck all cards are different)
            Assert.AreNotEqual(testTable.PlayerList[0].Pile[0].Name, testTable.PlayerList[1].Pile[0].Name);
            Assert.AreNotEqual(testTable.PlayerList[0].Pile[1].Name, testTable.PlayerList[1].Pile[1].Name);
            Assert.AreNotEqual(testTable.PlayerList[0].Pile[2].Name, testTable.PlayerList[1].Pile[2].Name);
        }

        [Test]
        public void ShuffleTest()
        {
            var testTable = new Table(3);
            var testDeck = testTable.Deck;
            Console.WriteLine(testTable.Deck[0].Name);
            testTable.Shuffle();
            Console.WriteLine(testTable.Deck[0].Name);
            //There is a very very small chance of this failing when the function is working
            //As technically the random shuffling could end up with an ordered deck, run again if test fails.
            Assert.AreNotEqual(testDeck, testTable.Deck);
        }
    }
}