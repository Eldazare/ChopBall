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
        //movement.SetCharacterAttributeData(CharacterAttributes);

        leftPaddle.SetCharacterBaseData(characterBase);
        //leftPaddle.SetCharacterAttributeData(CharacterAttributes);
        leftPaddle.SetPlayerID(PlayerID);

        rightPaddle.SetCharacterBaseData(characterBase);
        //rightPaddle.SetCharacterAttributeData(CharacterAttributes);
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

        LoadCharacterBase();
        InitializeComponentData();
    }

    private void FixedUpdate()
    {
        if (input != null)
        {
            if (input.PaddleLeft && !leftPaddleTriggeredLastFrame) leftPaddle.Hit();
            if (input.PaddleRight && !rightPaddleTriggeredLastFrame) rightPaddle.Hit();

            if (input.Dash && !dashTriggeredLastFrame)
            {
                movement.Dash(input.leftDirectionalInput);
            }

            if (input.leftDirectionalInput != Vector2.zero)
            {
                movement.Move(input.leftDirectionalInput);
            }

            if (input.rightDirectionalInput != Vector2.zero)
            {
                movement.Rotate(input.rightDirectionalInput);
            }

            leftPaddle.UpdatePaddle();
            rightPaddle.UpdatePaddle();

            leftPaddleTriggeredLastFrame = input.PaddleLeft;
            rightPaddleTriggeredLastFrame = input.PaddleRight;
            dashTriggeredLastFrame = input.Dash;
        }
    }
}
