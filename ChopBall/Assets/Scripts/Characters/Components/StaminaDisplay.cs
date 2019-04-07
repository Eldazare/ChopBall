using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour {

	public Image staminaCircle;
	public Image arrow;

	private CharacterRuntimeModifiers mod;
	private SpriteRenderer sRenderer;
	private bool initialized = false;
	private float maxStamina;

	public void Initialize(CharacterRuntimeModifiers mod, float maxStamina, Color32 color, Sprite shape){
		this.mod = mod;
		color.a = 180;
		if (staminaCircle == null) {
			staminaCircle = GetComponentInChildren<Image> ();
		}
		if (arrow != null) {
			arrow.color = color;
		}
		staminaCircle.color = color;
        staminaCircle.sprite = shape;
		this.maxStamina = maxStamina;
		initialized = true;
		//Debug.Log (maxStamina);
	}

	void Update(){
		if (initialized) {
			staminaCircle.fillAmount = mod.stamina / maxStamina;
			//Debug.Log (mod.stamina / maxStamina);
		}
	}
}
