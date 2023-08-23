using _SDK.UI;
using Assets._GAME.Scripts.Skills.Move;
using Assets._SDK.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInputPanel : AbstractPanel
{
	public VariableJoystick Joystick;
	public Button BtnRun;
	public Button BtnJump;
	public Button BtnLookBehind;

	bool _isRunActive = false;

	// Start is called before the first frame update
	void Start()
    {
		BtnRun?.onClick.AddListener(() => DetectButton(BtnRun, _isRunActive = !_isRunActive));

		DetectButton(BtnRun, _isRunActive);
	}

	public void DetectButton(Button btn, bool isActive)
	{
		btn.GetComponent<Image>().color = isActive ? Color.gray : Color.white;
	}

	public void ResetUI ()
	{
		_isRunActive = false;
		DetectButton(BtnRun, _isRunActive);
	}
}
