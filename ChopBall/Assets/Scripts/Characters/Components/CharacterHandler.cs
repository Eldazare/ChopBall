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

    private bool leftPaddleTriggeredLastFrame = false;
    private bool rightPaddleTriggeredLastFrame = false;
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
                characterBase = new CharacterBaseData();
            }
        }
    }

    private void InitializeComponentData()
    {
        movement.SetCharacterBaseData(characterBase);
        movement.SetCharacterAttributeData(CharacterAttributes);

        leftPaddle.SetCharacterBaseData(characterBase);
        leftPaddle.SetCharacterAttributeData(CharacterAttributes);
        leftPaddle.SetPlayerID(PlayerID);
		leftPaddle.Initialize ();

        rightPaddle.SetCharacterBaseData(characterBase);
        rightPaddle.SetCharacterAttributeData(CharacterAttributes);
        rightPaddle.SetPlayerID(PlayerID);
		rightPaddle.Initialize ();
    }

	private void LoadCharacterStates(){
		characterStates = CharacterStateController.GetCharStates ();
		currentState = characterStates [0];
	}

	public void Initialize()
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
        InitializeComponentData();
		LoadCharacterStates ();
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
                if (input.PaddleLeft && !leftPaddleTriggeredLastFrame) leftPaddle.Hit();
                if (input.PaddleRight && !rightPaddleTriggeredLastFrame) rightPaddle.Hit();

                trail.emitting = false;
            }

			// TODO: Implement currentState.blocking;
			// TODO: Implement state changes. Example:
			// currentState = characterStates[(int)CharacterStateEnum.Block];
			// can assume that characterStates are indexed according to the enum (check CharacterStateController for the check)

            //leftPaddle.UpdatePaddle();
            //rightPaddle.UpdatePaddle();

            leftPaddleTriggeredLastFrame = input.PaddleLeft;
            rightPaddleTriggeredLastFrame = input.PaddleRight;
            dashTriggeredLastFrame = input.Dash;
        }

        leftPaddle.UpdatePaddle();
        rightPaddle.UpdatePaddle();
    }
}
