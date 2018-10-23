using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterBaseData : ScriptableObject {

    // BASE DATA

    [Header("Paddles")]
    public float PaddleLength;
    public float PaddleThickness;
    public float PaddleUpperAngle;
    public float PaddleLowerAngle;
	public float PaddleSpeedUp;
	public float PaddleSpeedDown;
    public float PaddleForceAmount;
    public LayerMask PaddleCollisionLayers;

    [Header("Movement")]
	public float MovementSpeed;
    public float RotationSpeed;
    public float DashDistance;
	public float DashTime;
    public float DashCoolDown;
}
