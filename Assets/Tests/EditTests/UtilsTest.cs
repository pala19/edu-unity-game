using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;


namespace Tests
{
    public class UtilsTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void UtilsTestRandomAreNeverEqual()
        {
            var random1 = Utils.GetNewRandom();
            var random2 = Utils.GetNewRandom();
            Assert.AreNotEqual(random1, random2);
        }
        [Test]
        public void UtilsTestPermutationIsRandom()
        {
            var perm1 = Utils.GenerateRandomPermutation(5);
            var perm2 = Utils.GenerateRandomPermutation(5);
            Assert.AreNotEqual(perm1, perm2);
        }
    }
}
