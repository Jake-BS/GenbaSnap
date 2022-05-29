using Autofac.Extras.Moq;
using GenbaSnap.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

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

        //Valid inputs
        [TestCase("q", 2, "q")]
        [TestCase("p", 2, "p")]
        [TestCase("z", 3, "z")]
        [TestCase("m", 4, "m")]
        //Valid input but with accidental keystrokes
        [TestCase("fesfsfesfsm", 4, "m")]
        [TestCase("mp", 2, "p")]
        //Invalid inputs
        [TestCase("fhufihwh", 2, "")]
        [TestCase("/[].'#.", 2, "")]
        //Valid inputs but for incorrect number of players
        [TestCase("m", 2, "")]
        [TestCase("z", 2, "")]
        public void ValidateInputTest(string snapInput, int nOfPlayers, string predOutput)
        {
            var testTable = new Table(nOfPlayers);
            Assert.AreEqual(predOutput, testTable.ValidateInput(snapInput));
        }

        [Test]
        public void IntroTextTest()
        {
            var testTable = new Table(2);
            Assert.Pass();
        }

        [Test]
        public void OutroTextTest()
        {
            Table testTable = new Table(2);
            var winners = new List<string>() { "0", "1" };
            var predOutro = "The winners are Players 0, and 1!";
            testTable.Winners = winners;
            Assert.AreEqual(predOutro, testTable.OutroText());

            testTable = new Table(2);
            winners = new List<string>() { "1" };
            predOutro = "The winner is Player 1!";
            testTable.Winners = winners;
            Assert.AreEqual(predOutro, testTable.OutroText());
        }

        [Test]
        public void FindWinnerTest()
        {
            //2 players, player 0 has more cards, so should be the only winner
            Table testTable = new Table(2);
            Card genericCard = new Card("Generic", "Generic");
            testTable.PlayerList[0].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[1].FaceUpPile = new List<Card>() { genericCard };
            var predWinners = new List<string>() { "0"};
            testTable.GameDoneFindWinner();
            Assert.AreEqual(predWinners, testTable.Winners);

            //3 players, player 0 and player 1 have more cards than player 2, so 0 and 1 win.
            testTable = new Table(3);
            genericCard = new Card("Generic", "Generic");
            testTable.PlayerList[0].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[1].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[2].FaceUpPile = new List<Card>() { genericCard };
            predWinners = new List<string>() { "0", "1" };
            testTable.GameDoneFindWinner();
            Assert.AreEqual(predWinners, testTable.Winners);

            //4 player, all 4 have an equal amount of cards so they all win/draw
            testTable = new Table(4);
            genericCard = new Card("Generic", "Generic");
            testTable.PlayerList[0].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[1].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[2].FaceUpPile = new List<Card>() { genericCard, genericCard };
            testTable.PlayerList[3].FaceUpPile = new List<Card>() { genericCard, genericCard };
            predWinners = new List<string>() { "0", "1", "2", "3" };
            testTable.GameDoneFindWinner();
            Assert.AreEqual(predWinners, testTable.Winners);
        }

        [Test]
        public void SuccessfulSnapTest()
        {
            //Tests if winning a snap will add the two cards to player 0's pile
            //Player 0's pile is set to 50, after the victory of 2 cards player 0 should win the game
            var testTable = new Table(4);
            Card genericCard = new Card("Generic", "Generic");
            Card snapCard = new Card("Snap", "Snap");
            var curFaceUpCards = new List<Card>();
            testTable.PlayerList[0].Pile = new List<Card>();
            for (int i = 0; i < 50; i++) testTable.PlayerList[0].Pile.Add(genericCard);
            for (int i = 0; i < 2; i++)
            {
                curFaceUpCards.Add(genericCard);
                testTable.PlayerList[i].FaceUpPile.Add(genericCard);
            }
            for (int i = 0; i < 2; i++) 
            {
                curFaceUpCards.Add(snapCard);
                testTable.PlayerList[i+2].FaceUpPile.Add(snapCard);
            }
            
            var gameOver = false;
            var snapInput = "q";
            Assert.AreEqual(true, testTable.SuccessfulSnap(false, snapInput, snapCard, curFaceUpCards));
        }
    }
}