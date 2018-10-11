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

	public InputEvent[] inputEvents;
	public Material[] charMaterials;

	void Start () {
		CurrentBattleController.InitializeCurrentData ();
		PlayerStateData[] playerStates = PlayerStateController.GetAllStates ();
		GameObject goalMaster = GameObject.FindGameObjectWithTag ("GoalMaster");
		List<Transform> ballSpawns = GameObject.FindGameObjectWithTag ("BallSpawnMaster").GetComponentsInChildren<Transform>().ToList();
		ballSpawns.RemoveAt (0);
		PlayerBaseData playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		if (playerBaseData == null) {
			Debug.LogError ("playerBaseData not found");
			return;
		}
		inputEvents = Resources.LoadAll ("Scriptables/Input/Events/", typeof(InputEvent)).Cast<InputEvent>().ToArray ();
		charMaterials = Resources.LoadAll ("Materials", typeof(Material)).Cast<Material>().ToArray ();
		Goal[] goals = goalMaster.GetComponentsInChildren<Goal> ();

		Ball ballComponent = Instantiate (ball, Vector3.zero, Quaternion.identity).GetComponent<Ball>();
		ballComponent.Initialize (ballSpawns);
		ballComponent.ResetBallPosition ();

		// TODO: 
		int nextPlayerStateIndex = 0;
		bool areAnyActive = false;
		for(int i = 0; i < goals.Length; i++){
			while(nextPlayerStateIndex<playerStates.Length){
				if (playerStates [nextPlayerStateIndex].active) {
					areAnyActive = true;
					break;
				} else {
					nextPlayerStateIndex++;
				}
			}
			if (nextPlayerStateIndex >= playerStates.Length) {
				Debug.LogError ("Not enough active states found");
				if (!areAnyActive) {
					Debug.Log ("Loading defaults...");
				} else {
					break;
				}
			}
			if (!areAnyActive) {
				MakeACharacter (goals [i], null, playerBaseData, i);
			} else {
				MakeACharacter (goals[i], playerStates[nextPlayerStateIndex],playerBaseData, nextPlayerStateIndex);
				nextPlayerStateIndex++;
			}
		}

		StartGame.Raise();
	}

	private void MakeACharacter(Goal goal, PlayerStateData stateData, PlayerBaseData baseData, int playerIndex){
		goal.Initialize (playerIndex + 1, baseData.playerColors [playerIndex]);
		Vector3 charSpawnPos = goal.GetComponentInChildren<CharacterSpawnIndicator>().GetPosition();
		GameObject prefab = null;
		CharacterAttributeData charAttributes = null;
		if (stateData != null) {
			charAttributes = CharacterAttributeController.GetACharacter (stateData.characterChoice);
			if (charAttributes != null) {
				prefab = (GameObject)Resources.Load (CharacterAttributeController.GetCharacterPrefabPreString() + charAttributes.CharacterPrefabName);
			}
		}
		if (prefab == null) {
			prefab = characterTest;
		}
		CharacterHandler charHand = Instantiate (prefab, charSpawnPos, Quaternion.identity).GetComponent<CharacterHandler>();
		charHand.PlayerID = (playerIndex+1);
		InputEventListener charIEListener = charHand.GetComponent<InputEventListener> ();
		charIEListener.Event = inputEvents [playerIndex];
		charIEListener.enabled = true;
		if (stateData != null) {
			MeshRenderer[] renderers = charHand.GetComponentsInChildren<MeshRenderer> ();
			foreach (var renderer in renderers) {
				renderer.material = charMaterials [playerIndex];
				renderer.material.color = baseData.playerColors [playerIndex];
			}
		}
		charHand.Initialize ();
		charHand.CharacterAttributes = charAttributes;
	}
}
