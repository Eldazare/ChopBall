using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageChoiceType {individualRandom, masterSingle, randomPreset}
public enum GrandMode{ FFA , TEAMFFA , TeamVSTeam}

[CreateAssetMenu]
public class MasterStateData : ScriptableObject {
	// Runtime "Options" that reset 

	// TODO: Presets in the UI component. Might be stored as scriptable themselves.

	public GrandMode mode;
	public bool teams; // Wether the game is Free-For-All or Teams
	public int maxTeams;
	public BattleModeBlueprint battleModeBlueprint;

	public StageChoiceType stageChoiceType;
	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;

	public void SetBattleDefaults(){
		// Should contain every field
		SetGrandMode (GrandMode.FFA);
		battleModeBlueprint = DefaultBP ();
	}

	public void SetMenuDefaults(){
		stageChoiceType = StageChoiceType.masterSingle;
		stageNameFinal = "TestStage";
	}

	private BattleModeBlueprint DefaultBP(){
		BattleModeBlueprint newBP = new BattleModeBlueprint ();
		newBP.countObject = CountObject.Goals;
		newBP.roundEnd = RoundEnd.Timer;
		//newBP.roundEndCap = 0; // Starting stock or goalCap, per round
		newBP.timer = new ATime(1, 0f);
		newBP.endCriteria = MatchEnd.ScoreCap; // For ending the match
		newBP.endValue = 3;
		newBP.scoringMode = ScoringMode.WinnerOnly; // How do goals/Stocks/etc relate to score at end of round
		return newBP;
	}

	public void SetBattleModeBlueprint(BattleModeBlueprint bp){
		this.battleModeBlueprint = bp;
	}

	public void SetGrandMode (GrandMode mode){
		this.mode = mode;
		if (this.mode == GrandMode.FFA) {
			teams = false;
			maxTeams = 0;
		} else {
			teams = true;
			battleModeBlueprint.countObject = CountObject.Goals;
			if (this.mode == GrandMode.TEAMFFA) {
				maxTeams = 4;
			} else if (this.mode == GrandMode.TeamVSTeam) {
				maxTeams = 2;
			}
		}
	}

	public void GoToBattle(){
		foreach (StageData stageData in StageDataController.GetStages ()) {
			if (stageNameFinal == stageData.stageName) {
				SceneManager.LoadScene (stageData.stageSceneName);
				return;
			}
		}
		Debug.LogWarning ("Stage name not found: " + stageNameFinal);
	}
}
