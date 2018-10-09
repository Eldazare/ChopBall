using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleLoader : MonoBehaviour {

	public GameObject characterTest;
	public GameObject ball;
	//TODO: Reads StateData and initializes components, then invokes startGame
	//TODO: Initialize characters from StateData (and prefabs)
	//TODO: Initialize ball start positions from scene

	// TODO: PlayerState.CharID => CharName (CharAttributeData) => Load prefab with name
	// ---> Set charAttributeData

	public GameEvent StartGame;

	void Start () {
		CurrentBattleController.InitializeCurrentData ();
		PlayerStateData[] playerStates = PlayerStateController.GetAllStates ();
		Material[] charMaterials = Resources.LoadAll ("Materials", typeof(Material)).Cast<Material>().ToArray ();
		GameObject goalMaster = GameObject.FindGameObjectWithTag ("GoalMaster");
		Transform[] ballSpawns = GameObject.FindGameObjectWithTag ("BallSpawnMaster").GetComponentsInChildren<Transform>();
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
			CharacterHandler charHand = Instantiate (characterTest, charSpawnPos, Quaternion.identity).GetComponent<CharacterHandler>();
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
			nextPlayerStateIndex++;
		}

		StartGame.Raise();
	}
}
