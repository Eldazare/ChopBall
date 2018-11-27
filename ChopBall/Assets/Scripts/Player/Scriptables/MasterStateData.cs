using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum StageChoiceType {individualRandom, masterSingle, randomPreset}
public enum GrandMode{ FFA, TeamVSTeam } //TEAMFFA

[CreateAssetMenu]
public class MasterStateData : ScriptableObject {
	// Runtime "Options" that reset 

	// TODO: Presets in the UI component. Might be stored as scriptable themselves.

	public GameEvent OnGrandModeChanged;

	public GrandMode mode = GrandMode.FFA;
	public int quickBpIndex = -1;
	public bool teams; // Wether the game is Free-For-All or Teams
	public int maxTeams;
	public BattleModeBlueprint battleModeBlueprint;

	public StageChoiceType stageChoiceType;
	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;

	public void SetBattleDefaults(){
		SetGrandMode (GrandMode.FFA);
		if (quickBpIndex == -1) {
			battleModeBlueprint = DefaultBP ();
		} else {
			LoadBlueprintFromQuickMode ();
		}
		Debug.LogWarning ("Defaults loaded, assumed debug mode.");
	}

	public void SetMenuDefaults(){
		stageChoiceType = StageChoiceType.masterSingle;
		stageNameFinal = "1v1 Test";
		quickBpIndex = -1;
	}

	private BattleModeBlueprint DefaultBP(){
		BattleModeBlueprint newBP = new BattleModeBlueprint ();
		newBP.countObject = CountObject.Lives;
		newBP.roundEnd = RoundEnd.Timer;
		newBP.roundEndCap = 3; // Starting stock or goalCap, per round
		newBP.timer = new ATime(0, 30f);
		newBP.endCriteria = MatchEnd.ScoreCap; // For ending the match
		newBP.endValue = 10;
		newBP.scoringMode = ScoringMode.Direct1to1; // How do goals/Stocks/etc relate to score at end of round
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
			//battleModeBlueprint.countObject = CountObject.Goals;
			/*
			if (this.mode == GrandMode.TEAMFFA) {
				maxTeams = 4;
			} else*/
			if (this.mode == GrandMode.TeamVSTeam) {
				maxTeams = 2;
			}
		}
		OnGrandModeChanged.Raise ();
	}

	public void GoToBattle(){
		LoadBlueprintFromQuickMode ();
		foreach (StageData stageData in StageDataController.GetStages ()) {
			if (stageNameFinal == stageData.stageMenuName) {
				SceneManager.LoadScene (stageData.stageSceneName);
				return;
			}
		}
		Debug.LogWarning ("Stage name not found: " + stageNameFinal);
	}

	private void LoadBlueprintFromQuickMode(){
		if (quickBpIndex != -1) {
			battleModeBlueprint = QuickModeBPController.GetAllBPs () [quickBpIndex].GetBlueprint ();
		} else {
			battleModeBlueprint = DefaultBP ();
		}
	}
}
