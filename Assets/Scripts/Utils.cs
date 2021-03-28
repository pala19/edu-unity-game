using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils
{
    
    public static int[] GenerateRandomPermutation(int n)
    {
        int[] ResultPermutation = new int[n];
        ResultPermutation = Enumerable.Range(0, n+1).ToArray();
        System.Random random = GetNewRandom();

        return ResultPermutation.OrderBy(x => random.Next()).ToArray();
    }
    public static System.Random GetNewRandom()
    {
        return new System.Random(System.Guid.NewGuid().GetHashCode());
    }


}
