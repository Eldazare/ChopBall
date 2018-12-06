using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModel {

	public int playerID;
	public Vector2 leftDirectionalInput;
	public Vector2 rightDirectionalInput;

	public bool Strike;
	public bool Dash;
	public bool Block;

	public bool Submit;
	public bool Cancel;

	public bool Start;
	public bool Select;

	public Vector2 D_PadVector;

	public InputModel(){
		leftDirectionalInput = Vector2.zero;
		rightDirectionalInput = Vector2.zero;

		Strike = false;
		Dash = false;
		Block = false;

		Submit = false;
		Cancel = false;

		Start = false;
		Select = false;

		D_PadVector = Vector2.zero;
	}
}
