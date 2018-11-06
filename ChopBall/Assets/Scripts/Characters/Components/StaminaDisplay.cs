using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaDisplay : MonoBehaviour {

	public Image image;

	private CharacterRuntimeModifiers mod;
	private SpriteRenderer sRenderer;
	private bool initialized = false;
	private float maxStamina;

	public void Initialize(CharacterRuntimeModifiers mod, float maxStamina, Color32 color){
		this.mod = mod;
		if (image == null) {
			image = GetComponentInChildren<Image> ();
		}
		image.color = color;
		this.maxStamina = maxStamina;
		initialized = true;
		//Debug.Log (maxStamina);
	}

	void Update(){
		if (initialized) {
			image.fillAmount = mod.stamina / maxStamina;
			//Debug.Log (mod.stamina / maxStamina);
		}
	}
}
