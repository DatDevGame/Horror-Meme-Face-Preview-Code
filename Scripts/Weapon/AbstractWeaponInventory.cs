using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets._SDK.Weapon
{
    public abstract class AbstractWeaponInventory
    {
        public AbstractWeapon PlayingWeapon { get; protected set; }
        public AbstractWeapon EquippedWeapon { get; protected set; }
        public abstract void Load();
    }
}
