using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageChoiceType {individualRandom, masterSingle, randomPreset}

[CreateAssetMenu]
public class MasterStateData : ScriptableObject {
	// Runtime "Options" that reset 

	// TODO: Presets in the UI component. Might be stored as scriptable themselves.


	public int numberOfPlayers; // TODO: Define how this is read / input by player?
	public bool teams; // Wether the game is Free-For-All or Teams
	public BattleModeBlueprint battleModeBlueprint;

	public StageChoiceType stageChoiceType;
	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;

	public void SetDefaults(){
		// Should contain every field
		numberOfPlayers = 1;
		teams = false;
		battleModeBlueprint = DefaultBP ();
		stageChoiceType = StageChoiceType.masterSingle;
		stageNameFinal = "TestStage";
	}

	private BattleModeBlueprint DefaultBP(){
		BattleModeBlueprint newBP = new BattleModeBlueprint ();
		//TODO: set defaults
		return newBP;
	}

	public void SetBattleModeBlueprint(BattleModeBlueprint bp){
		this.battleModeBlueprint = bp;
	}

	public void GoToBattle(){
		foreach (StageData stageData in StageDataController.GetStages ()) {
			if (stageNameFinal == stageData.stageName) {
				SceneManager.LoadScene (stageData.stageSceneName);
				break;
			}
		}
		Debug.LogWarning ("Stage name not found: " + stageNameFinal);
	}
}
