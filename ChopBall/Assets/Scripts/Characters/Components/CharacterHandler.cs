using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHandler : MonoBehaviour {

    public int PlayerID;
    public CharacterAttributeData CharacterAttributes;
	public CharacterRuntimeModifiers CharacterRuntimeModifiers;
	public MeshRenderer[] bodyRenderers;
    public ParticleSystem TrailParticles;
    public ParticleSystem DashParticles;
    public bool CasualControls = true;
    public bool RotateToMoveDir = true;

    private CharacterMovement movement;
    private CharacterPaddle leftPaddle;
    private CharacterPaddle rightPaddle;
    private InputModel input;

    private TrailRenderer trail;

    private bool leftPaddleInputLastFrame = false;
    private bool rightPaddleInputLastFrame = false;
    private bool dashTriggeredLastFrame = false;
	private bool blockInputLastFrame = false;

	private float rightPaddleCharge;
	private float leftPaddleCharge;

    private CharacterBaseData characterBase;

	private CharacterState currentState;
	private CharacterState[] characterStates;

	private string soundPlayerHit;
	private Vector3 startPosition;
	private float startRot;
	private bool startPosRotSet = false;
	private bool paused = false;

    public void SetInputModel(InputModel model)
    {
        this.input = model;
    }

	public void Pause(){
		if (paused) {
			paused = false;
		} else {
			paused = true;
		}
	}

    public void SetPositionAndRotation(Vector2 position, float rotation)
    {
        movement.SetPositionAndRotation(position, rotation);
		SetStartPosAndRot (position, rotation);
    }

	public void SetStartPosAndRot (Vector3 startPosition, float startRot){
		if (!startPosRotSet) {
			this.startPosition = startPosition;
			this.startRot = startRot;
			startPosRotSet = true;
		}
	}

	public void ResetToStartPosition(){
        TrailParticles.Stop();
		transform.position = startPosition;
		transform.rotation = Quaternion.Euler(new Vector3(0,0, startRot));
        TrailParticles.Play();
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
		ResetToStartPosition ();
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
		LoadCharacterStates();
		soundPlayerHit = SoundPathController.GetPath ("PlayerHit");
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
		if (paused) {
			movement.Move (Vector2.zero);
			return;
		}
        if (input != null)
        {
			if (CasualControls)
            {
                if (input.PaddleLeft && !leftPaddleInputLastFrame && currentState.canDash)
                {
                    if (CharacterRuntimeModifiers.UseStamina(characterBase.DashStaminaCost))
                    {
                        movement.Dash(input.leftDirectionalInput);
                        DashParticles.Play();
                    }
                }

                movement.Move(input.leftDirectionalInput * currentState.stateMovementModifier);
                if (RotateToMoveDir) movement.Rotate(input.leftDirectionalInput);
                else movement.Rotate(input.rightDirectionalInput);

                if (movement.isDashing)
                {
                    trail.startWidth = 1f + Mathf.Abs(Vector2.Dot(transform.up, movement.velocity.normalized));
                    trail.endWidth = trail.startWidth;
                    trail.emitting = true;
                }
                else if (currentState.canPaddle)
                {
                    if (input.PaddleRight)
                    {
                        if (!rightPaddleInputLastFrame)
                        {
                            rightPaddle.Hit();
                            leftPaddle.Hit();
                        }
                        else if (!rightPaddle.hitActive)
                        {
                            //Debug.Log("Charging right");
                            rightPaddle.isCharging = true;
                            leftPaddle.isCharging = true;
                            if (currentState.identifier != CharacterStateEnum.Charge) TransitionToState(CharacterStateEnum.Charge);
                        }
                    }
                    else
                    {
                        if (rightPaddleInputLastFrame && rightPaddle.isCharging)
                        {
                            if (CharacterRuntimeModifiers.UseStamina(characterBase.PaddleChargedStaminaCost))
                            {
                                //Debug.Log("Charge shot right");
                                rightPaddle.Hit(rightPaddleCharge < 0);
                                leftPaddle.Hit(leftPaddleCharge < 0);
                            }
                            rightPaddle.isCharging = false;
                            leftPaddle.isCharging = false;
                            TransitionToState(CharacterStateEnum.Default);
                            rightPaddleCharge = 1f;
                            leftPaddleCharge = 1f;
                        }
                    }

                    trail.emitting = false;
                }

                //if (input.Block && !blockInputLastFrame)
                //{
                //    TransitionToState(CharacterStateEnum.Block);
                //    Debug.Log("Block Registered");
                //}

                //if (!input.Block && blockInputLastFrame)
                //{
                //    TransitionToState(CharacterStateEnum.Default);
                //}

                // TODO: Implement currentState.blocking;
                // TODO: Implement state changes. (Transition method is done)

                leftPaddleInputLastFrame = input.PaddleLeft;
                rightPaddleInputLastFrame = input.PaddleRight;
                //dashTriggeredLastFrame = input.Dash;
                //blockInputLastFrame = input.Block;

                if (leftPaddle.isCharging)
                {
                    leftPaddleCharge -= Time.fixedDeltaTime;
                }

                if (rightPaddle.isCharging)
                {
                    rightPaddleCharge -= Time.fixedDeltaTime;
                }

                //Debug.Log("State: " + currentState);
            }
            else
            {
                if (input.Dash && !dashTriggeredLastFrame && currentState.canDash)
                {
                    if (CharacterRuntimeModifiers.UseStamina(characterBase.DashStaminaCost))
                    {
                        movement.Dash(input.leftDirectionalInput);
                    }
                }

                movement.Move(input.leftDirectionalInput * currentState.stateMovementModifier);
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
                        if (!leftPaddleInputLastFrame)
                        {
                            leftPaddle.Hit();
                        }
                        else if (!leftPaddle.hitActive)
                        {
                            //Debug.Log("Charging left");
                            leftPaddle.isCharging = true;
                            if (currentState.identifier != CharacterStateEnum.Charge) TransitionToState(CharacterStateEnum.Charge);
                        }
                    }
                    else
                    {
                        if (leftPaddleInputLastFrame && leftPaddle.isCharging)
                        {
                            if (CharacterRuntimeModifiers.UseStamina(characterBase.PaddleChargedStaminaCost))
                            {
                                //Debug.Log("Charge shot left");
                                leftPaddle.Hit(leftPaddleCharge < 0);
                            }
                            leftPaddle.isCharging = false;
                            TransitionToState(CharacterStateEnum.Default);
                            leftPaddleCharge = 1f;
                        }
                    }
                    if (input.PaddleRight)
                    {
                        if (!rightPaddleInputLastFrame)
                        {
                            rightPaddle.Hit();
                        }
                        else if (!rightPaddle.hitActive)
                        {
                            //Debug.Log("Charging right");
                            rightPaddle.isCharging = true;
                            if (currentState.identifier != CharacterStateEnum.Charge) TransitionToState(CharacterStateEnum.Charge);
                        }
                    }
                    else
                    {
                        if (rightPaddleInputLastFrame && rightPaddle.isCharging)
                        {
                            if (CharacterRuntimeModifiers.UseStamina(characterBase.PaddleChargedStaminaCost))
                            {
                                //Debug.Log("Charge shot right");
                                rightPaddle.Hit(rightPaddleCharge < 0);
                            }
                            rightPaddle.isCharging = false;
                            TransitionToState(CharacterStateEnum.Default);
                            rightPaddleCharge = 1f;
                        }
                    }

                    trail.emitting = false;
                }

                if (input.Block && !blockInputLastFrame)
                {
                    TransitionToState(CharacterStateEnum.Block);
                    Debug.Log("Block Registered");
                }

                if (!input.Block && blockInputLastFrame)
                {
                    TransitionToState(CharacterStateEnum.Default);
                }

                // TODO: Implement currentState.blocking;
                // TODO: Implement state changes. (Transition method is done)

                leftPaddleInputLastFrame = input.PaddleLeft;
                rightPaddleInputLastFrame = input.PaddleRight;
                dashTriggeredLastFrame = input.Dash;
                blockInputLastFrame = input.Block;

                if (leftPaddle.isCharging)
                {
                    leftPaddleCharge -= Time.fixedDeltaTime;
                }

                if (rightPaddle.isCharging)
                {
                    rightPaddleCharge -= Time.fixedDeltaTime;
                }

                //Debug.Log("State: " + currentState);
            }
        }

        leftPaddle.UpdatePaddle();
        rightPaddle.UpdatePaddle();
    }

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.CompareTag ("Ball") && currentState.blocking) {
			Ball ball = collision.collider.GetComponent<Ball> ();
			if (ball.GetComponent<Rigidbody2D> ().velocity.magnitude > characterBase.MinimumVelocityStamina) {
				if (ball.IsCharged ()) {
					if (CharacterRuntimeModifiers.UseStamina (characterBase.ChargeBlockStaminaCost)) {
						ball.OnBlocked (collision.contacts [0].normal, characterBase.BlockForceDampModifier);
					}
				} else if (CharacterRuntimeModifiers.UseStamina (characterBase.BlockStaminaCost)) {
					ball.OnBlocked (collision.contacts [0].normal, characterBase.BlockForceDampModifier);
				}
			} else {
				ball.OnBlocked (collision.contacts [0].normal, characterBase.BlockForceDampModifier);
			}
			FMODUnity.RuntimeManager.PlayOneShotAttached (soundPlayerHit, gameObject);
		}
	}
}
