﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModel {

	public int playerID;
	public float XAxisLeft;
	public float YAxisLeft;
	public float XAxisRight;
	public float YAxisRight;
	public Vector2 leftDirection;
	public Vector2 rightDirection;

	public bool PaddleLeft;
	public bool PaddleRight;
	public bool Dash;

	public bool Submit;
	public bool Cancel;

	// Default = Zero;
	public InputModel(){
		XAxisLeft = 0;
		YAxisLeft = 0;
		XAxisRight = 0;
		YAxisRight = 0;
		leftDirection = Vector2.zero;
		rightDirection = Vector2.zero;

		PaddleLeft = false;
		PaddleRight = false;
		Dash = false;

		Submit = false;
		Cancel = false;
	}
}
