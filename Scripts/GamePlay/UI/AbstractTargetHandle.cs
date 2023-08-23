using _GAME.Scripts.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractTargetHandle : MonoBehaviour
{
    [SerializeField] protected Sprite AchieveSpriteDefault;
    [SerializeField] protected GameObject targetPrefab;
    protected List<GameObject> itemList = new List<GameObject>();

    public virtual void AddTarget(int targetWin)
    {
        foreach (GameObject child in itemList)
            Destroy(child);

        itemList.Clear();
        for (int i = 0; i < targetWin; i++)
        {
            GameObject objItem = Instantiate(targetPrefab, Vector3.zero, Quaternion.identity, this.transform);
            objItem.GetComponentInChildren<Image>().sprite = AchieveSpriteDefault;
            itemList.Add(objItem);
        }
    }
    public abstract void TargetHandle(float currentTarget, Skin skin = null);
}
