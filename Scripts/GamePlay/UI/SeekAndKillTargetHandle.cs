using _GAME.Scripts.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class SeekAndKillTargetHandle : AbstractTargetHandle
{
    private Sprite EnemySpriteKilled;
    public override void AddTarget(int targetWin)
    {
        base.AddTarget(targetWin);
    }
    public override void TargetHandle(float currentTarget, Skin skin = null)
    {
        var item = this.transform.GetChild((int)currentTarget - 1);
        if (item == null) return;

        EnemySpriteKilled = skin.Image;
        var items = item.GetComponent<Image>();
        items.sprite = EnemySpriteKilled;
        items.color = new Color(1, 1, 1, 0.7f);
        item.transform.GetChild(0).gameObject.SetActive(true);
    }

	public void TargetHandleGallery(float currentTarget, Skin skin = null)
	{
		TargetHandle(currentTarget, skin);
		var item = this.transform.GetChild((int)currentTarget - 1);
		if (item == null) return;

		item.transform.GetChild(1)?.gameObject.SetActive(true);
	}
}
