﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CursorBaseData : ScriptableObject {

	public float movespeedMultiplier;
	public float dashMovespeedMultiplier;
	public int pixelBuffer;

	// Master Cursor Data
	public float McTotalMultiplierCap;
}
