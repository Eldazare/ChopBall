using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BattleLoader : MonoBehaviour {


	// Currently Initializes shared goals as last player's goal in team list.

	public GameObject characterTest;
	public GameObject goalDefenseTarget;
	public GameObject ball;
	public float betweenTargets;
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


	CharacterBaseData charBaseData;

	void Start () {
		charBaseData = (CharacterBaseData)Resources.Load ("Scriptables/_BaseDatas/CharacterBaseData");
		RuntimeModifierController.ClearAttributeDatas ();
		GrandMode mode = MasterStateController.GetTheMasterData ().mode;
		PlayerStateData[] playerStates = PlayerStateController.GetAllStates ();
		GameObject goalMaster = GameObject.FindGameObjectWithTag ("GoalMaster");
		Goal[] goals = goalMaster.GetComponentsInChildren<Goal> ();
		CurrentBattleController.InitializeCurrentData (goals.Length);
		List<Transform> ballSpawns = GameObject.FindGameObjectWithTag ("BallSpawnMaster").GetComponentsInChildren<Transform>().ToList();
		ballSpawns.RemoveAt (0);
		PlayerBaseData playerBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		if (playerBaseData == null) {
			Debug.LogError ("playerBaseData not found");
			return;
		}
		inputEvents = InputEventController.GetAllEvents ();
		charMaterials = Resources.LoadAll ("Materials", typeof(Material)).Cast<Material>().ToArray ();
		int balls = goals.Length-1;
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

		// TEAMVSTEAM Predistribution
		List<List<int>> teams = new List<List<int>> (4);
		if (mode == GrandMode.TeamVSTeam) {
			foreach (var team in CurrentBattleController.GetTeams()) {
				teams.Add (team.playerIndexes);
			}
			teams.RemoveAll (r => r.Count == 0);
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
			Color32 theColor = new Color32(0,0,0,0);
			if (!areAnyActive) {
				theColor.a = 0;
				MakeACharacter (goals [i], Vector2.zero, null, new Color32(255,255,255,255), i);
			} else if (mode == GrandMode.FreeForAll) {
				if (activeStates.Count > i) {
					theColor = playerBaseData.playerColors [activeStates [i]];
					MakeACharacter (goals [i], Vector2.zero, playerStates [activeStates [i]], theColor , activeStates [i]);
				}
			} 
			/*else if (mode == GrandMode.TEAMFFA) {
				if (teams.Count > i) {
					theColor = playerBaseData.teamColors [playerStates [teams [i] [0]].team];
					MakeMultipleCharacters (goals [i], teams [i], playerStates, theColor);
				}

			}*/ 
			else if (mode == GrandMode.TeamVSTeam) {
				if (playersInSpawnPoints.Count > i) {
					if (playersInSpawnPoints [i].Count > 0) {
						Debug.Log ("i: " + i + " | spawpoints: " + playersInSpawnPoints [i].Count);
						theColor = playerBaseData.teamColors [playerStates [playersInSpawnPoints [i] [0]].team];
						MakeMultipleCharacters (goals [i], playersInSpawnPoints [i], playerStates, theColor);
					}
				}
			}
			Material mat = new Material (Shader.Find ("Standard"));
			mat.color = theColor;
			goals [i].Initialize (theColor, mat, i);
			GenerateTargets (goals [i], mat);
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
		goal.InitializeID (playerIndex + 1);
		//Vector3 charSpawnPos = goal.GetComponentInChildren<CharacterSpawnIndicator> ().GetPosition ();
		GameObject prefab = null;
		CharacterAttributeData charAttributes = null;
		if (stateData != null) {
			charAttributes = CharacterAttributeController.GetACharacter (stateData.characterChoice);
			if (charAttributes != null) {
				RuntimeModifierController.AddAttributeData (charAttributes, playerIndex);
				prefab = (GameObject)Resources.Load (CharacterAttributeController.GetCharacterPrefabPreString () + charAttributes.CharacterPrefabName);
			}
		}
		if (prefab == null) {
			prefab = characterTest;
			charAttributes = CharacterAttributeController.GetDefaultChar ();
			RuntimeModifierController.AddAttributeData (charAttributes, playerIndex);
		}
		CharacterHandler charHand = Instantiate (prefab, new Vector3(0,0,0), Quaternion.identity).GetComponent<CharacterHandler> ();
		//charHand.transform.Translate (relativePos);
		//charHand.transform.Rotate(new Vector3(0,0,-90)); // Difference between default goal and default character rotations
		charHand.PlayerID = (playerIndex+1);
		InputEventListener charIEListener = charHand.GetComponent<InputEventListener> ();
		charIEListener.Event = inputEvents [playerIndex];
		charIEListener.enabled = true;
		charMaterials [playerIndex].color = theColor;
		if (stateData != null) {
			MeshRenderer[] renderers = charHand.bodyRenderers;
			foreach (var renderer in renderers) {
				renderer.material = charMaterials [playerIndex];
			}
		}
		charHand.CharacterAttributes = CharacterAttributeController.GetDefaultChar(); // BIG: Change this to get "real" stats
		charHand.CharacterRuntimeModifiers = RuntimeModifierController.GetAMod (playerIndex + 1);
		charHand.Initialize (theColor);
		goal.SetCharPosAndRot (charHand, relativePos);


		// DEBUG
		float staminaMax = charBaseData.StaminaMax;
		if (charAttributes != null) {
			staminaMax *= charAttributes.StaminaMax;
		}
		charHand.GetComponent<StaminaDisplay> ().Initialize (RuntimeModifierController.GetAMod (playerIndex + 1), staminaMax, theColor);
	}

	private void GenerateTargets(Goal goal, Material targetMat){
		Vector3 middlePos = goal.GetComponentInChildren<DefenseTargetSpawnIndicatior> ().GetPosition ();
		float ySizeTarget = goalDefenseTarget.GetComponentInChildren<Renderer>().bounds.size.y + betweenTargets;
		float ySizeGoal = goal.GetComponent<BoxCollider2D> ().bounds.size.y / 2;
		int targetCount = Mathf.FloorToInt((ySizeGoal * 2) / ySizeTarget);
		Vector3 leftPos = middlePos + goal.transform.up  * (ySizeTarget / 2f)*(targetCount-1);
		Vector3 currentPos = leftPos;
		for (int i = 0; i < targetCount; i++) {
			GameObject theTarget = (GameObject)Instantiate (goalDefenseTarget, currentPos, goal.transform.rotation);
			theTarget.GetComponentInChildren<MeshRenderer> ().material = targetMat;
			currentPos += goal.transform.up * -1 * ySizeTarget;
			goal.AddTargetToGoal (theTarget.GetComponent<GoalTarget>());
		}
	}
}
