using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode{shortStock, longStock, goalsOnly};

[CreateAssetMenu]
public class CurrentBattleStorage : ScriptableObject {
	public bool useTimer = false; //
	public int minutesLeft;
	public float secondsLeft;
	public int roundsLeft;
	public List<TeamContainer> teamData;
	public List<CompetitorContainer> competitorData; // All Players (even ones with a team)

	public void LoadFromMasterState(){
		MasterStateData masterData = MasterStateController.GetTheMasterData ();
		if (masterData.teams) {
			teamData = new List<TeamContainer> ();
			// TODO: Iterate playerdata and get teamID:s
		} else {
			teamData = null;
		}
		// TODO: Iterate playerdata and Generate CompetitorContainers

		if (masterData.timer.used) {
			useTimer = true;
			minutesLeft = masterData.timer.minutes;
			secondsLeft = masterData.timer.seconds;
		} else {
			useTimer = false;
		}
	}
}

public class TeamContainer{
	public int teamID;
	public int score;
}

public class CompetitorContainer{
	public float score;
	public int shortStock;
	public int longStock;

	public int goalsScored;
}