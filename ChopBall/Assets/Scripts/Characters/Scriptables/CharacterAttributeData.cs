using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterAttributeData : ScriptableObject {

    // ATTRIBUTE DATA

    [Header("External")]
	public string CharacterName;
	public string CharacterPrefabName;
	public GameObject CharacterBattleModelPrefab;
	public GameObject CharacterMenuModelPrefab;
	public List<Sprite> CharacterPortraits;

	[Header("Color palettes")]
	public List<PaletteContainer> Palettes;

	[Header("General")]
	public float StaminaMax = 1f;
	public float StaminaRegen = 1f; 

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
    public float BodyMassMultiplier = 1f;
    public float LinearDragMultiplier = 1f;
}

[System.Serializable]
public class PaletteContainer{
	public Material hair;
	public Material skin;
	public Material eyes;
}