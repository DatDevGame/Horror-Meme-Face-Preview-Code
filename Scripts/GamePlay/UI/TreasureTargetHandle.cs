using _GAME.Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreasureTargetHandle : AbstractTargetHandle
{
    public override void AddTarget(int targetWin)
    {
        base.AddTarget(targetWin);
    }
    public override void TargetHandle(float currentTarget, Skin skin = null)
    {
        var item = this.transform.GetChild((int)currentTarget - 1);
        if (item == null) return;

        item.GetComponent<Image>().enabled = false;
        item.transform.GetChild(0).gameObject.SetActive(true);
    }
}
