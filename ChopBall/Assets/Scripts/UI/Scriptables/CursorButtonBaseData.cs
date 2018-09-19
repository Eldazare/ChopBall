using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CursorButtonBaseData : ScriptableObject {

	// General Choice button datan
	public Color32 HoverHilightColor;
	public Color32 NeutralHilightColor;



	// StageChoiceButton data
	public Color32 ScbMaskHilight;
	public Color32 ScbMaskNeutral;
	public Color32 ScbMaskDarken;
	public Color32 ScbOutlineColor;
}
