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
        System.Random random = new System.Random(System.Guid.NewGuid().GetHashCode());

        return ResultPermutation.OrderBy(x => random.Next()).ToArray();
    }


}
