using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CountGameDataTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CountGameDataTestWithPressedButtonCheck()
        {
            Assert.IsTrue(CountGameData.PressedButton);
            CountGameData.PressedButton = false;
            Assert.IsFalse(CountGameData.PressedButton);
        }

    }
}
