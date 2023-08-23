using _SDK.Shop;
using Assets._SDK.Shop;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionShop : AbstractShop<IShopItem>
{
    public override void Load()
    {
        Items = GameManager.Instance.Resources.AllMissionSettings.Values.Select(settings => (IShopItem)settings.Entity).ToList();
    }
}
