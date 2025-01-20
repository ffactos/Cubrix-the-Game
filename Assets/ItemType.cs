using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemType
{
    public enum Type
    {
        Weapon,
        Resource
    }
    
    public enum Weapon
    {
        Melee,
        Range,
        Angel,
        Demon
    }
    public enum Resource
    {
        Wood,
        Stone,
        Iron,
        Gold
    }
}
