using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModel {

	public int playerID;
	public Vector2 leftDirectionalInput;
	public Vector2 rightDirectionalInput;

	public bool PaddleLeft;
	public bool PaddleRight;
	public bool Dash;
	public bool Block;

	public bool Submit;
	public bool Cancel;

	public bool Start;
	public bool Select;

	public bool D_PadUp;
	public bool D_PadDown;
	public bool D_PadLeft;
	public bool D_PadRight;

	public InputModel(){
		leftDirectionalInput = Vector2.zero;
		rightDirectionalInput = Vector2.zero;

		PaddleLeft = false;
		PaddleRight = false;
		Dash = false;
		Block = false;

		Submit = false;
		Cancel = false;

		Start = false;
		Select = false;

		D_PadUp = false;
		D_PadDown = false;
		D_PadLeft = false;
		D_PadRight = false;
	}
}
