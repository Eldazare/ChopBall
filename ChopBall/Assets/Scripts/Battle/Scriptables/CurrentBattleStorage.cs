using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class CurrentBattleStorage : ScriptableObject {
	// Flow:
	// MasterStateData: Stores which gamemode is current and all necessary parameters
	// Loader initializes static gateway to the correct ScriptableObjects and correct "displays"
	// All gamemodes are child scriptables from a collection of commands


	public bool useTimer = false; //
	public int minutesLeft;
	public float secondsLeft;
	public int roundsLeft;
	public int roundsToWin;
	public List<TeamContainer> teamData;
	public List<CompetitorContainer> competitorData; // All Players (even ones with a team)

	private MasterStateData masterData;

	public void LoadFromMasterState(){
		masterData = MasterStateController.GetTheMasterData ();
		//roundsLeft = masterData.totalRounds;

		PlayerStateData[] playerStates = PlayerStateController.GetAllStates();
		if (masterData.teams) {
			teamData = new List<TeamContainer> ();
			foreach (PlayerStateData stateData in playerStates) {
				teamData.Add (null);
			}
		} else {
			teamData = null;
		}
		// TODO: Iterate playerdata and Generate CompetitorContainers
		foreach (PlayerStateData stateData in playerStates) {
			CompetitorContainer newCompCont = new CompetitorContainer ();
			//newCompCont.stock = masterData.stocks;
			newCompCont.score = 0;
			newCompCont.goalsScored = 0;
			if (masterData.teams) {
				newCompCont.teamID = stateData.team;
				teamData [stateData.team] = new TeamContainer (stateData.team);
			} else {
				newCompCont.teamID = -1;
			}
			competitorData.Add (newCompCont);
		}
		/*
		if (masterData.timer.used) {
			useTimer = true;
			minutesLeft = masterData;
			secondsLeft = masterData.timer.seconds;
		} else {
			useTimer = false;
		}
		*/
	}

	public void DoGoal(GoalData gd){
		competitorData [gd.giverPlayerID].DidAGoal ();
		competitorData [gd.goalPlayerID].RemoveStock ();
		if (teamData != null) {
			teamData [gd.giverPlayerID].TeamDidAGoal ();
		}
	}

	public void RoundEnd(){
		roundsLeft -= 1;
		foreach (CompetitorContainer cc in competitorData) {
			cc.score += cc.stock;
			if (cc.teamID > -1) {
			}
		}
		
	}
}

public class TeamContainer{
	public TeamContainer(int iD){
		teamID = iD;
		goals = 0;
		score = 0;
	}

	public void TeamDidAGoal(){
		goals += 1;
	}

	public void AddScore(int amount){
		score += amount;
	}

	public int teamID;
	public int goals;
	public int score;
}

public class CompetitorContainer{
	public int teamID;
	public float score;
	public int stock;
	public int goalsScored;

	public void DidAGoal(){
		goalsScored += 1;
	}

	public void RemoveStock(){
		stock -= 1;
		if (stock < 0) {
			stock = 0;
		}
	}
}