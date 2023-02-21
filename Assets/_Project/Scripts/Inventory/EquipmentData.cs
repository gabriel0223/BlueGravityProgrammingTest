using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class EquipmentData : ItemData
{
    public enum EquipmentType
    {
        Face, Head, Top, Bottom, Weapon
    }
    
    public EquipmentType equipmentType;
    public Sprite equipmentSprite;
}
