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
            Assert.AreEqual(numberOfPlayers, testTable.PlayerList.Count);
        }
    }
}