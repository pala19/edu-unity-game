using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<int> CountingGameClearedLevels = new List<int>();
    public List<int> AddGameClearedLevels = new List<int>();
    public List<int> SubGameClearedLevels = new List<int>();
    public List<int> BasketGameClearedLevels = new List<int>();

}
