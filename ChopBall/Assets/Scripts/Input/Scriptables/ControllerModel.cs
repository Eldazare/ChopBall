using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ControllerModel : ScriptableObject{

	// Stores listened axes as axis number
	// Stores default buttons as button number

	public string ControllerName;
	[Header("Input Axes")]
	public int XAxisLeft;
	public int YAxisLeft;
	public int XAxisRight;
	public int YAxisRight;
	public float deadZone = 0.19f;

	[Header("Main buttons")]
	public int PaddleLeft;
	public int PaddleRight;
	public int Dash;
	public int Block;
	public int Submit;
	public int Cancel;
	public int Start;
	public int Select;

	[Header("Button Axes")]
	public int DashAxis;
	public int BlockAxis;

	[Header("D-pad Axes")]
	public int D_PadX;
	public int D_PadY;
}
