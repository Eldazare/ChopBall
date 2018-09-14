using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModel {

	public int playerID;
	public Vector2 leftDirection;
	public Vector2 rightDirection;

	public bool PaddleLeft;
	public bool PaddleRight;
	public bool Dash;

	public bool Submit;
	public bool Cancel;

	// Default = Zero;
	public InputModel(){
		leftDirection = Vector2.zero;
		rightDirection = Vector2.zero;

		PaddleLeft = false;
		PaddleRight = false;
		Dash = false;

		Submit = false;
		Cancel = false;
	}
}
