using _GAME.Scripts.Inventory;
using _SDK.UI;
using Assets._GAME.Scripts.Skills.Live;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Game;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingMainPanel : GamePlayingPanel
{
	#region Survival
	public void UpdateTargetSurvival(float value)
	{
		SurvivalTargetHandle.TargetHandle(value);
	}
	#endregion

	#region Treasure
	public void StartUITreasure(double targetToWin)
	{
		TreasureTargetHandle.AddTarget((int)targetToWin);
	}
	public void UpdateTargetTreasure(float targetCurrent)
	{
		TreasureTargetHandle.TargetHandle(targetCurrent);
	}
	#endregion

	#region Seek And Kill
	public void StartUISeekAndKill(double targetToWin)
	{
		SeekAndKillTarget.AddTarget((int)targetToWin);
	}
	public void UpdateTargetSeekAndKill(float targetCurrent, Skin skin)
	{
		SeekAndKillTarget.TargetHandle(targetCurrent, skin);
	}
	public void UpdateTargetGallerySeekAndKill(float targetCurrent, Skin skin)
	{
		SeekAndKillTarget.TargetHandleGallery(targetCurrent, skin);
	}
	#endregion

	#region Seek And Kill Collect
	public void StartUISeekAndKillCollect(double targetToWin)
	{
		SeekAndKillCollectTarget.AddTarget((int)targetToWin);
	}

	public void UpdateKeysFoundSeekAndKill(int totalKeysFound)
	{
		SeekAndKillCollectTarget.TargetHandle((float)totalKeysFound);
	}
	#endregion
}
