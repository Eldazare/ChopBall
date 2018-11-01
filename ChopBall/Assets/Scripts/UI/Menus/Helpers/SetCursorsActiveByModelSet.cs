using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursorsActiveByModelSet : MonoBehaviour {

	public MenuPanelHandler mph;

	void OnEnable(){
		for(int i = 0; i < InputTranslatorMaster.modelsPut.Length;i++){
			mph.playerCursors [i].gameObject.SetActive (InputTranslatorMaster.modelsPut [i]);
		}
	}
}
