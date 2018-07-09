using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    Any,
    Handheld,
    Hat,
    Armour,
    Boots
}

public enum HandHeldTypes
{
    NotApplicable,
    Slashing,
    Piercing,
    Bashing,
    Shield
}

public class ItemDetails : MonoBehaviour {

    public ItemTypes ItemType;
    public HandHeldTypes HandHeldType;
}
