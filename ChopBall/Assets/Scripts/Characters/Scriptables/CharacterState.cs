using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStateEnum{Default, Block, Charge}

[CreateAssetMenu]
public class CharacterState : ScriptableObject {
	public CharacterStateEnum identifier;
	public float stateMovementModifier;
	public float staminaRegenBuffMagnitude;
	public bool canPaddle;
	public bool canDash;
	public bool blocking;

	private StaminaRegenBuff stamBuff;

	private void Initialize(){
		if (stamBuff == null) {
			stamBuff = new StaminaRegenBuff (0, staminaRegenBuffMagnitude);
		}
	}

	public void OnStateEnter(int playerID){
		Initialize ();
		BuffController.AddBuff (playerID, stamBuff);
	}

	public void OnStateExit(int playerID){
		Initialize ();
		BuffController.EndBuff (playerID, stamBuff);
	}
}
