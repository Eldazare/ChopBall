using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStateEnum{Default, Block, Charge}

[CreateAssetMenu]
public class CharacterState : ScriptableObject {
	public CharacterStateEnum identifier;
	public float stateMovementModifier;
	public float stateMassModifier;
	public bool canPaddle;
	public bool canDash;
	public bool blocking;

	private void Initialize(){
	}

	public void OnStateEnter(int playerID){
		Initialize ();
	}

	public void OnStateExit(int playerID){
		Initialize ();
	}
}
