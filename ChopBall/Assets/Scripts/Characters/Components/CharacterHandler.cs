using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    public int PlayerID;
    public CharacterAttributeData CharacterAttributes;
	public CharacterRuntimeModifiers CharacterRuntimeModifiers;

    private CharacterMovement movement;
    private CharacterPaddle leftPaddle;
    private CharacterPaddle rightPaddle;
    private InputModel input;

    private TrailRenderer trail;

    private bool leftPaddleInputLastFrame = false;
    private bool rightPaddleInputLastFrame = false;
    private bool dashTriggeredLastFrame = false;

    private CharacterBaseData characterBase;

	private CharacterState currentState;
	private CharacterState[] characterStates;

    public void SetInputModel(InputModel model)
    {
        this.input = model;
    }

    public void SetPositionAndRotation(Vector2 position, float rotation)
    {
        movement.SetPositionAndRotation(position, rotation);
    }

    private void LoadCharacterBase()
    {
        if (characterBase == null)
        {
            characterBase = (CharacterBaseData)Resources.Load("Scriptables/_BaseDatas/CharacterBaseData", typeof(CharacterBaseData));
            
            if (characterBase == null)
            {
				Debug.LogWarning ("FFUUU");
                characterBase = new CharacterBaseData();
            }
        }
    }

	private void InitializeComponentData(Color32 theColor)
    {
        movement.SetCharacterBaseData(characterBase);
        movement.SetCharacterAttributeData(CharacterAttributes);

        leftPaddle.SetCharacterBaseData(characterBase);
        leftPaddle.SetCharacterAttributeData(CharacterAttributes);
		leftPaddle.SetRuntimeModifiers (CharacterRuntimeModifiers);
        leftPaddle.SetPlayerID(PlayerID);
		leftPaddle.Initialize (theColor);

        rightPaddle.SetCharacterBaseData(characterBase);
        rightPaddle.SetCharacterAttributeData(CharacterAttributes);
		rightPaddle.SetRuntimeModifiers (CharacterRuntimeModifiers);
        rightPaddle.SetPlayerID(PlayerID);
		rightPaddle.Initialize (theColor);
    }

	private void LoadCharacterStates(){
		characterStates = CharacterStateController.GetCharStates ();
		TransitionToState (CharacterStateEnum.Default);
	}

	public void Initialize(Color32 theColor)
    {
        CharacterPaddle[] paddles = new CharacterPaddle[2];
        paddles = GetComponents<CharacterPaddle>();
		for (int i = 0; i < paddles.Length; i++)
		{
			if (paddles[i].Side == CharacterPaddle.PaddleSide.Left)
			{
				leftPaddle = paddles[i];
			}
			else
			{
				rightPaddle = paddles[i];
			}
		}
        movement = GetComponent<CharacterMovement>();

        trail = GetComponentInChildren<TrailRenderer>();

        LoadCharacterBase();
		InitializeComponentData(theColor);
		LoadCharacterStates ();
    }

	private void TransitionToState(CharacterStateEnum enu){
		if (currentState != null) {
			currentState.OnStateExit (PlayerID);
		}
		currentState = characterStates [(int)enu];
		currentState.OnStateEnter (PlayerID);
		movement.SetRigidbodyMass (currentState.stateMassModifier * characterBase.BodyMass);
	}

    private void FixedUpdate()
    {
        if (input != null)
        {
			if (input.Dash && !dashTriggeredLastFrame && currentState.canDash)
            {
                movement.Dash(input.leftDirectionalInput);
            }

			movement.Move(input.leftDirectionalInput*currentState.stateMovementModifier);
            movement.Rotate(input.rightDirectionalInput);

			if (movement.isDashing)
			{
				trail.startWidth = 1f + Mathf.Abs(Vector2.Dot(transform.up, movement.velocity.normalized));
				trail.endWidth = trail.startWidth;
				trail.emitting = true;
			}
			else if (currentState.canPaddle)
            {
                if (input.PaddleLeft)
                {
					if (!leftPaddleInputLastFrame) {
						leftPaddle.Hit ();
					}
                    else if (!leftPaddle.hitActive)
                    {
                        //Debug.Log("Charging left");
                        leftPaddle.isCharging = true;
                        if (!rightPaddle.isCharging) TransitionToState(CharacterStateEnum.Charge);
                    }
                }
                else
                {
                    if (leftPaddleInputLastFrame && leftPaddle.isCharging)
                    {
                        //Debug.Log("Charge shot left");
                        leftPaddle.Hit(true);
                        leftPaddle.isCharging = false;
                        if (!rightPaddle.isCharging) TransitionToState(CharacterStateEnum.Default);
                    }
                }
                if(input.PaddleRight)
                {
					if (!rightPaddleInputLastFrame) {
						rightPaddle.Hit ();
					}
                    else if (!rightPaddle.hitActive)
                    {
                        //Debug.Log("Charging right");
                        rightPaddle.isCharging = true;
                        if (!leftPaddle.isCharging) TransitionToState(CharacterStateEnum.Charge);
                    }
                }
                else
                {
                    if (rightPaddleInputLastFrame && rightPaddle.isCharging)
                    {
                        //Debug.Log("Charge shot right");
                        rightPaddle.Hit(true);
                        rightPaddle.isCharging = false;
                        if (!leftPaddle.isCharging) TransitionToState(CharacterStateEnum.Default);
                    }
                }

                trail.emitting = false;
            }

			if (input.Block) {
				Debug.Log ("Block Registered");
			}

			// TODO: Implement currentState.blocking;
			// TODO: Implement state changes. (Transition method is done)

            //leftPaddle.UpdatePaddle();
            //rightPaddle.UpdatePaddle();

            leftPaddleInputLastFrame = input.PaddleLeft;
            rightPaddleInputLastFrame = input.PaddleRight;
            dashTriggeredLastFrame = input.Dash;

            //Debug.Log("State: " + currentState);
        }

        leftPaddle.UpdatePaddle();
        rightPaddle.UpdatePaddle();
    }
}
