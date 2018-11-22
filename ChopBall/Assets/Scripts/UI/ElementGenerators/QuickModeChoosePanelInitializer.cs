using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickModeChoosePanelInitializer : MonoBehaviour {

	public MenuPanelHandler mph;
	public Text descriptionText;
	public PanelScript nextPanel;

	void Awake(){
		ChooseGameModeControlButton[] buts = GetComponentsInChildren<ChooseGameModeControlButton> ();
		for (int i = 0; i < buts.Length; i++) {
			buts [i].Initialize (i, descriptionText, mph, nextPanel);
		}
	}
}
