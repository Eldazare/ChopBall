using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    private CharacterMovement movement;
    private CharacterPaddle[] paddles;
    private InputModel input;

    private bool leftPaddleTriggeredLastFrame = false;
    private bool rightPaddleTriggeredLastFrame = false;

    public void GetInputModel(InputModel model)
    {
        this.input = model;
    }

    private void Awake()
    {
        paddles = new CharacterPaddle[2];
        paddles = GetComponents<CharacterPaddle>();
        movement = GetComponent<CharacterMovement>();
    }

    private void FixedUpdate()
    {
        if (input != null)
        {
            if (input.PaddleLeft && !leftPaddleTriggeredLastFrame) paddles[0].Hit();
            if (input.PaddleRight && !rightPaddleTriggeredLastFrame) paddles[1].Hit();

            if (input.Dash)
            {
                movement.Dash();
            }

            movement.Move(input.leftDirection);
            movement.Rotate(input.rightDirection);

            leftPaddleTriggeredLastFrame = input.PaddleLeft;
            rightPaddleTriggeredLastFrame = input.PaddleRight;
        }
    }
}
