using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleLoader : MonoBehaviour {


	// Currently Initializes shared goals as last player's goal in team list.
	// TODO: Properly organize players around spawnpoint.

	public GameObject characterTest;
	public GameObject ball;
	//Reads StateData and initializes components, then invokes startGame

	// TODO: TEST: PlayerState.CharID => CharName (CharAttributeData) => Load prefab with name
	// ---> Set charAttributeData
	// TODO: TEST for null checks

	public GameEvent StartGame;

	public InputEvent[] inputEvents;
	public Material[] charMaterials;

	public static float distanceFromCenter = 2;

	private Vector2[] modifierPositions = {new Vector2 (distanceFromCenter, distanceFromCenter), new Vector2 (distanceFromCenter, -distanceFromCenter),
		new Vector2 (-distanceFromCenter, -distanceFromCenter), new Vector2 (-distanceFromCenter, distanceFromCenter),
	};

	void Start () {
		CurrentBattleController.InitializeCurrentData ();
		GrandMode mode = MasterStateController.GetTheMasterData ().mode;
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
		int balls = goals.Length / 2;
		if (balls < 1) {
			balls = 1;
		}
		for (int i = 0; i<balls;i++){
			Ball ballComponent = Instantiate (ball, Vector3.zero, Quaternion.identity).GetComponent<Ball>();
			ballComponent.Initialize (ballSpawns);
			ballComponent.ResetBallPosition ();
		}



		// TeamVSTeam Predistribution





		// TODO: Predistribution
		List<int> activeStates = new List<int>();
		int nextPlayerStateIndex = 0;
		bool areAnyActive = false;
		while (true) {
			while (nextPlayerStateIndex < playerStates.Length) {
				if (playerStates [nextPlayerStateIndex].active) {
					areAnyActive = true;
					activeStates.Add (nextPlayerStateIndex);
					break;
				} else {
					nextPlayerStateIndex++;
				}
			}
			if (nextPlayerStateIndex >= playerStates.Length) {
				Debug.LogError ("Not enough active states found");
				if (!areAnyActive) {
					Debug.Log ("Loading defaults...");
				} 
				break;
			}
		}

		// Team Predistribution
		List<List<int>> teams = new List<List<int>>();
		for (int i = 0; i < 4; i++) {
			teams.Add (new List<int> ());
		}
		foreach (var indexi in activeStates) {
			teams [playerStates [indexi].team].Add (indexi);
		}
		teams.RemoveAll (r => r.Count == 0);

		// TEAMVSTEAM Predistribution
		List<List<int>> playersInSpawnPoints = new List<List<int>> ();
		if (mode == GrandMode.TeamVSTeam) {
			for (int i = 0; i < goals.Length;i++){
				playersInSpawnPoints.Add (new List<int> ());
			}
			int half = goals.Length / 2;
			foreach (var ind in teams[0]) {
				for (int i = 0; i < half; i++) {
					int previous = i - 1;
					if (previous == (0-1)){
						previous = half-1;
					}
					if (playersInSpawnPoints [i].Count == playersInSpawnPoints [previous].Count) {
						playersInSpawnPoints [i].Add (ind);
						break;
					}
				}
			}
			foreach (var ind in teams[1]) {
				for (int i = half; i < (half*2); i++) {
					int previous = i - 1;
					if (previous == (half-1)){
						previous = (half*2)-1;
					}
					if (playersInSpawnPoints [i].Count == playersInSpawnPoints [previous].Count) {
						playersInSpawnPoints [i].Add (ind);
						break;
					}
				}
			}
		}
					
		for(int i = 0; i < goals.Length; i++){
			if (!areAnyActive) {
				MakeACharacter (goals [i], Vector2.zero, null, new Color32(255,255,255,255), i);
			} else if (mode == GrandMode.FFA) {
				MakeACharacter (goals [i], Vector2.zero, playerStates [activeStates[i]], playerBaseData.playerColors[activeStates[i]], activeStates[i]);
			} else if (mode == GrandMode.TEAMFFA) {
				Color32 color = playerBaseData.teamColors [playerStates [teams[i][0]].team];
				MakeMultipleCharacters (goals [i], teams [i], playerStates, color);
			} else if (mode == GrandMode.TeamVSTeam) {
				Color32 color = playerBaseData.teamColors [playerStates[playersInSpawnPoints[i][0]].team];
				MakeMultipleCharacters(goals[i], playersInSpawnPoints[i], playerStates, color);
			}
			nextPlayerStateIndex++;
		}
		StartGame.Raise();
	}

	private void MakeMultipleCharacters (Goal goal, List<int> stateIndexes, PlayerStateData[] stateDatas, Color32 theColor){
		for (int i = 0; i < stateIndexes.Count; i++) {
			Vector2 modPos = modifierPositions [i];
			if (stateIndexes.Count == 1) {
				modPos = Vector2.zero;
			} else if (stateIndexes.Count == 2) {
				modPos.x -= distanceFromCenter;
			}
			MakeACharacter (goal, modPos, stateDatas [stateIndexes[i]], theColor, stateIndexes[i]);
		}
	}

	private void MakeACharacter(Goal goal, Vector2 relativePos, PlayerStateData stateData, Color32 theColor, int playerIndex){
		goal.Initialize (playerIndex + 1, theColor);
		Vector3 charSpawnPos = goal.GetComponentInChildren<CharacterSpawnIndicator> ().GetPosition ();
		GameObject prefab = null;
		CharacterAttributeData charAttributes = null;
		if (stateData != null) {
			charAttributes = CharacterAttributeController.GetACharacter (stateData.characterChoice);
			if (charAttributes != null) {
				prefab = (GameObject)Resources.Load (CharacterAttributeController.GetCharacterPrefabPreString () + charAttributes.CharacterPrefabName);
			}
		}
		if (prefab == null) {
			prefab = characterTest;
		}
		CharacterHandler charHand = Instantiate (prefab, charSpawnPos, goal.transform.rotation).GetComponent<CharacterHandler> ();
		charHand.transform.Translate (relativePos);
		charHand.transform.Rotate(new Vector3(0,0,-90)); // Difference between default goal and default character rotations
		charHand.PlayerID = (playerIndex+1);
		InputEventListener charIEListener = charHand.GetComponent<InputEventListener> ();
		charIEListener.Event = inputEvents [playerIndex];
		charIEListener.enabled = true;
		if (stateData != null) {
			MeshRenderer[] renderers = charHand.GetComponentsInChildren<MeshRenderer> ();
			foreach (var renderer in renderers) {
				renderer.material = charMaterials [playerIndex];
				renderer.material.color = theColor;
			}
		}
		charHand.Initialize ();
		charHand.CharacterAttributes = charAttributes;
	}
}
