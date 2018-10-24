using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    public int PlayerID;
    public CharacterAttributeData CharacterAttributes;

    private CharacterMovement movement;
    private CharacterPaddle leftPaddle;
    private CharacterPaddle rightPaddle;
    private InputModel input;

    private TrailRenderer trail;

    private bool leftPaddleTriggeredLastFrame = false;
    private bool rightPaddleTriggeredLastFrame = false;
    private bool dashTriggeredLastFrame = false;

    private CharacterBaseData characterBase;

    public void SetInputModel(InputModel model)
    {
        this.input = model;
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

        rightPaddle.SetCharacterBaseData(characterBase);
        rightPaddle.SetCharacterAttributeData(CharacterAttributes);
        rightPaddle.SetPlayerID(PlayerID);
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
    }

    private void FixedUpdate()
    {
        if (input != null)
        {
            if (input.Dash && !dashTriggeredLastFrame)
            {
                movement.Dash(input.leftDirectionalInput);
            }

            movement.Move(input.leftDirectionalInput);
            movement.Rotate(input.rightDirectionalInput);

            if (!movement.isDashing)
            {
                if (input.PaddleLeft && !leftPaddleTriggeredLastFrame) leftPaddle.Hit();
                if (input.PaddleRight && !rightPaddleTriggeredLastFrame) rightPaddle.Hit();

                trail.emitting = false;
            }
            else
            {
                trail.startWidth = 1f + Mathf.Abs(Vector2.Dot(transform.up, movement.velocity.normalized));
                trail.endWidth = trail.startWidth;
                trail.emitting = true;
            }

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
