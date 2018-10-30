using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterStateEnum{Default, Block, Charge}

[CreateAssetMenu]
public class CharacterState : ScriptableObject {
	public CharacterStateEnum identifier;
	public float stateMovementModifier;
	public bool canPaddle;
	public bool canDash;
	public bool blocking;
}
