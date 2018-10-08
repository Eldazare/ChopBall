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

	public bool Submit;
	public bool Cancel;

	public bool Start;
	public bool Select;

	// Default = Zero;
	public InputModel(){
		leftDirectionalInput = Vector2.zero;
		rightDirectionalInput = Vector2.zero;

		PaddleLeft = false;
		PaddleRight = false;
		Dash = false;

		Submit = false;
		Cancel = false;

		Start = false;
		Select = false;
	}
}
