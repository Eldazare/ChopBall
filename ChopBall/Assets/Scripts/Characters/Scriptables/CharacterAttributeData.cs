using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterAttributeData : ScriptableObject {

    // ATTRIBUTE DATA

    [Header("External")]
	public string CharacterName;
	public string CharacterPrefabName;
	public Sprite CharacterPortrait;

    [Header("Paddles")]
    public float PaddleLengthMultiplier = 1f;
    public float PaddleThicknessMultiplier = 1f;
    public float PaddleUpperAngleMultiplier = 1f;
    public float PaddleLowerAngleMultiplier = 1f;
    public float PaddleSpeedUpMultiplier = 1f;
    public float PaddleSpeedDownMultiplier = 1f;
    public float PaddleForceMultiplier = 1f;

    [Header("Movement")]
    public float MovementSpeedMultiplier = 1f;
    public float RotationSpeedMultiplier = 1f;
    public float DashDistanceMultiplier = 1f;
    public float DashTimeMultiplier = 1f;
    public float DashCoolDownMultiplier = 1f;
}
