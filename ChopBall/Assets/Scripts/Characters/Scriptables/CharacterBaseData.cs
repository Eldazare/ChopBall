using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterBaseData : ScriptableObject {

    // BASE DATA

	[Header("General")]
	public float StaminaMax;
	public float StaminaRegen;

	public float PaddleStaminaCost;
	public float PaddleChargedStaminaCost;
	public float DashStaminaCost;
	public float BlockStaminaCost;
	public float ChargeBlockStaminaCost;

	[Header("PlayerInteraction")]
	public float InvunerabilityTime;
	public float DashForceMultiplier;
	public float PaddleForceMultiplier;

    [Header("Paddles")]
    public float PaddleLength;
    public float PaddleThickness;
    public float PaddleUpperAngle;
    public float PaddleLowerAngle;
	public float PaddleSpeedUp;
	public float PaddleSpeedDown;
    public float PaddleForceAmount;
	public float PaddleChargedForceMultiplier;
    public LayerMask PaddleCollisionLayers;

    [Header("Movement")]
	public float MovementSpeed;
    public float RotationSpeed;
    public float DashDistance;
	public float DashTime;
    public float DashCoolDown;
    public float BodyMass;
    public float LinearDrag;

	[Header("Blocking")]
	public float BlockForceDampModifier;
	public float MinimumVelocityStamina;
}
