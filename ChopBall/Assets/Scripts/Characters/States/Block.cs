﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : _CharState {

	override
	public CharacterState Input(InputModel model){
		return CharacterState.None;
	}
	override
	public void OnStateEnter(){
		
	}

	override
	public void OnStateExit(){
		
	}
}
