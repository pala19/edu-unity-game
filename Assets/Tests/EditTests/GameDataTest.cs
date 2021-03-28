﻿using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameDataTest
    {
        private int NumberOfRounds = 6;
        // A Test behaves as an ordinary method
        [Test]
        public void CountGameDataTestWithPressedButtonCheck()
        {
            Assert.IsTrue(CountGameData.PressedButton);
            CountGameData.PressedButton = false;
            Assert.IsFalse(CountGameData.PressedButton);
        }
        [Test]
        public void AddGameDataTestWithPressedButtonCheck()
        {
            Assert.IsTrue(AddGameData.PressedButton);
            AddGameData.PressedButton = false;
            Assert.IsFalse(AddGameData.PressedButton);
        }
        [Test]
        public void SubGameDataTestWithPressedButtonCheck()
        {
            Assert.IsTrue(SubGameData.PressedButton);
            SubGameData.PressedButton = false;
            Assert.IsFalse(SubGameData.PressedButton);
        }
        [Test]
        public void CountGameDataTestWithSetCurrentGameCheck()
        {
            var curr1 = CountGameData.GetCurrentGame;
            CountGameData.SetCurrentGame = curr1 + 1;
            Assert.AreNotEqual(curr1, CountGameData.GetCurrentGame);
        }
        [Test]
        public void AddGameDataTestWithSetCurrentGameCheck()
        {
            var curr1 = AddGameData.GetCurrentGame;
            AddGameData.SetCurrentGame = curr1 + 1;
            Assert.AreNotEqual(curr1, AddGameData.GetCurrentGame);
        }
        [Test]
        public void SubGameDataTestWithSetCurrentGameCheck()
        {
            var curr1 = SubGameData.GetCurrentGame;
            SubGameData.SetCurrentGame = curr1 + 1;
            Assert.AreNotEqual(curr1, SubGameData.GetCurrentGame);
        }
        [Test]
        public void GameDataTestLanguageCheck()
        {
            Assert.AreEqual(MainGameData.changeLanguage, Application.systemLanguage);
        }
        [Test]
        public void CountGameDataTestIsActiveCheck()
        {
            Assert.IsTrue(CountGameData.IsActive(0));
            for (int i = 1; i < NumberOfRounds; i++)
            {
                Assert.IsFalse(CountGameData.IsActive(i));
            }
        }
        [Test]
        public void AddGameDataTestIsActiveCheck()
        {
            for (int i = 0; i < NumberOfRounds; i++)
            {
                Assert.IsFalse(AddGameData.IsActive(i));
            }
        }
        [Test]
        public void SubGameDataTestIsActiveCheck()
        {
            for (int i = 0; i < NumberOfRounds; i++)
            {
                Assert.IsFalse(SubGameData.IsActive(i));
            }
        }


    }
}
