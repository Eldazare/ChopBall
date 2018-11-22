using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseGameModeControlButton : _ControlButton {

	public Text buttonText;

	private int modeIndex;
	private Text descriptionText;
	private MenuPanelHandler mph;
	private PanelScript nextPanel;

	public void Initialize(int modeIndex, Text descriptionText, MenuPanelHandler mph, PanelScript nextPanel){
		this.modeIndex = modeIndex;
		buttonText.text = QuickModeBPController.GetNamePerIndex (modeIndex);
		this.descriptionText = descriptionText;
		this.mph = mph;
		this.nextPanel = nextPanel;
	}

	override
	public void OnButtonClick(int playerID){
		ChooseGamemode ();
		mph.Forward (nextPanel);
	}

	override
	public void OnButtonEnter(int playerID){
		descriptionText.text = QuickModeBPController.GetDescriptionPerIndex (modeIndex);
	}

	private void ChooseGamemode(){
		MasterStateController.GetTheMasterData ().quickBpIndex = modeIndex;
	}
}
