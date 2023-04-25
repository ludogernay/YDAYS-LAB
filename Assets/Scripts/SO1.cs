using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "so", menuName = "so", order = 1)]
public class SO1 : ScriptableObject
{
    public bool win;
    public List<int> liste = new List<int>(){1,2,3,4};
}
