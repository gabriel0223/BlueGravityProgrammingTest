using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : ScriptableObject
{
    public string name;
    public int purchasePrice;
    public int sellingPrice;
    [TextArea(5, 5)] public string description;
}
