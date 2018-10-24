using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleLoader : MonoBehaviour {


	// Currently Initializes shared goals as last player's goal in team list.

	public GameObject characterTest;
	public GameObject goalDefenseTarget;
	public GameObject ball;
	public int betweenTargets;
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

	private List<List<int>> playersInSpawnPoints;

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

		// Predistribution
		List<int> activeStates = new List<int>();
		int nextPlayerStateIndex = 0;
		bool areAnyActive = false;
		while (true) {
			while (nextPlayerStateIndex < playerStates.Length) {
				if (playerStates [nextPlayerStateIndex].active) {
					areAnyActive = true;
					activeStates.Add (nextPlayerStateIndex);
					break;
				}
				nextPlayerStateIndex++;
			}
			if (nextPlayerStateIndex >= playerStates.Length) {
				//Debug.LogError ("Not enough active states found");
				if (!areAnyActive) {
					Debug.Log ("Loading defaults...");
				} 
				break;
			}
			nextPlayerStateIndex++;
		}

		Debug.Log ("Active states found: " + activeStates.Count);

		// Team Predistribution
		List<List<int>> teams = new List<List<int>> (4);
		if (mode == GrandMode.TEAMFFA || mode == GrandMode.TeamVSTeam) {
			foreach (var team in CurrentBattleController.GetTeams()) {
				teams.Add (team.playerIndexes);
			}
			teams.RemoveAll (r => r.Count == 0);
		}

		// TEAMVSTEAM Predistribution
		if (mode == GrandMode.TeamVSTeam) {
			playersInSpawnPoints = new List<List<int>> ();
			if (teams.Count == 2) {
				for (int i = 0; i < goals.Length; i++) {
					playersInSpawnPoints.Add (new List<int> ());
				}
				int half = goals.Length / 2;
				DivideTeamIntoSpawnPoints (teams [0], 0, half);
				DivideTeamIntoSpawnPoints (teams [1], half, half * 2);
			} else {
				Debug.LogError ("Incorrect team amount for TeamVSTeam: " + teams.Count);
				return;
			}
		}
					
		for(int i = 0; i < goals.Length; i++){
			if (!areAnyActive) {
				MakeACharacter (goals [i], Vector2.zero, null, new Color32(255,255,255,255), i);
			} else if (mode == GrandMode.FFA) {
				if (activeStates.Count > i) {
					MakeACharacter (goals [i], Vector2.zero, playerStates [activeStates [i]], playerBaseData.playerColors [activeStates [i]], activeStates [i]);
				}
			} else if (mode == GrandMode.TEAMFFA) {
				if (teams.Count > i) {
					Color32 color = playerBaseData.teamColors [playerStates [teams [i] [0]].team];
					MakeMultipleCharacters (goals [i], teams [i], playerStates, color);
				}
			} else if (mode == GrandMode.TeamVSTeam) {
				if (playersInSpawnPoints.Count > i) {
					if (playersInSpawnPoints [i].Count > 0) {
						Debug.Log ("i: " + i + " | spawpoints: " + playersInSpawnPoints [i].Count);
						Color32 color = playerBaseData.teamColors [playerStates [playersInSpawnPoints [i] [0]].team];
						MakeMultipleCharacters (goals [i], playersInSpawnPoints [i], playerStates, color);
					}
				}
			}
			nextPlayerStateIndex++;
		}
		StartGame.Raise();
	}

	private void DivideTeamIntoSpawnPoints(List<int> team, int bas, int cap){
		foreach (var ind in team) {
			for (int i = bas; i < cap; i++) {
				int next = i + 1;
				if (next == cap) {
					next = bas;
				}
				if (playersInSpawnPoints [i].Count == playersInSpawnPoints [next].Count) {
					playersInSpawnPoints [i].Add (ind);
					break;
				}
				if (i == cap - 1 && playersInSpawnPoints [i].Count == playersInSpawnPoints [next].Count - 1) {
					playersInSpawnPoints [i].Add (ind);
					break;
				}
			}
		}
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
			charAttributes = CharacterAttributeController.GetDefaultChar ();
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
		charHand.CharacterAttributes = charAttributes;
		//charHand.CharRuntimeMods = RuntimeModifierController.GetAMod (playerIndex + 1);
		charHand.Initialize ();
	}

	private void GenerateTargets(Goal goal){
		
	}
}
