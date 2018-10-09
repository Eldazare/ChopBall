using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleLoader : MonoBehaviour {

	public GameObject characterTest;
	public GameObject ball;
	//Reads StateData and initializes components, then invokes startGame

	// TODO: TEST: PlayerState.CharID => CharName (CharAttributeData) => Load prefab with name
	// ---> Set charAttributeData
	// TODO: TEST for null checks

	public GameEvent StartGame;

	void Start () {
		CurrentBattleController.InitializeCurrentData ();
		PlayerStateData[] playerStates = PlayerStateController.GetAllStates ();
		Material[] charMaterials = Resources.LoadAll ("Materials", typeof(Material)).Cast<Material>().ToArray ();
		GameObject goalMaster = GameObject.FindGameObjectWithTag ("GoalMaster");
		List<Transform> ballSpawns = GameObject.FindGameObjectWithTag ("BallSpawnMaster").GetComponentsInChildren<Transform>().ToList();
		ballSpawns.RemoveAt (0);
		PlayerBaseData playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		if (playerBaseData == null) {
			Debug.LogError ("playerBaseData not found");
			return;
		}
		InputEvent[] inputEvents = Resources.LoadAll ("Scriptables/Input/Events/", typeof(InputEvent)).Cast<InputEvent>().ToArray ();
		Goal[] goals = goalMaster.GetComponentsInChildren<Goal> ();

		Ball ballComponent = Instantiate (ball, Vector3.zero, Quaternion.identity).GetComponent<Ball>();
		ballComponent.Initialize (ballSpawns);
		ballComponent.ResetBallPosition ();

		// TODO: 
		int nextPlayerStateIndex = 0;
		for(int i = 0; i < goals.Length; i++){
			while(nextPlayerStateIndex<playerStates.Length){
				if (playerStates [nextPlayerStateIndex].active) {
					break;
				} else {
					nextPlayerStateIndex++;
				}
			}
			if (nextPlayerStateIndex >= playerStates.Length) {
				Debug.LogError ("Not enough active states found");
				break;
			}
			goals [i].Initialize ((nextPlayerStateIndex + 1), playerBaseData.playerColors [nextPlayerStateIndex]);

			// NOT FINAL Character initialization code
			Vector3 charSpawnPos = goals [i].GetComponentInChildren<CharacterSpawnIndicator> ().GetPosition ();
			CharacterAttributeData charAttributes = CharacterAttributeController.GetACharacter (playerStates [nextPlayerStateIndex].characterChoice);
			GameObject charPrefab;
			if (charAttributes != null){
				charPrefab = (GameObject)Resources.Load(CharacterAttributeController.GetCharacterPrefabPreString() + charAttributes.CharacterPrefabName);
			}
			else {
				charPrefab = characterTest;
			}
			CharacterHandler charHand = Instantiate (charPrefab, charSpawnPos, Quaternion.identity).GetComponent<CharacterHandler>();
			charHand.PlayerID = (nextPlayerStateIndex+1);
			InputEventListener charIEListener = charHand.GetComponent<InputEventListener> ();
			charIEListener.Event = inputEvents [nextPlayerStateIndex];
			charIEListener.enabled = true;
			MeshRenderer[] renderers = charHand.GetComponentsInChildren<MeshRenderer> ();
			foreach (var renderer in renderers) {
				renderer.material = charMaterials [i];
				renderer.material.color = playerBaseData.playerColors [i];
			}
			charHand.Initialize ();
			charHand.CharacterAttributes = charAttributes;
			nextPlayerStateIndex++;
		}

		StartGame.Raise();
	}
}
