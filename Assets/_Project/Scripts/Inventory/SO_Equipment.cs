using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class SO_Equipment : SO_Item
{
    public enum EquipmentType
    {
        Head, Face, Top, Bottom, Weapon
    }
    
    public EquipmentType equipmentType;
    public Sprite equipmentSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
