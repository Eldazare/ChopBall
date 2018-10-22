using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CountObject{Stocks, Goals}
public enum RoundEnd{Elimination, Cap, Timer}
public enum MatchEnd{Rounds, ScoreCap}
public enum ScoringMode{WinnerOnly, PerPosition, Direct1to1}

[CreateAssetMenu]
public class BattleMode : ScriptableObject {

	int MAXNUMBEROFTEAMS = 4;

	public GameEvent EndOfRound;
	public GameEvent EndOfMatch;

	public GameEvent StatsUpdated;
	public GameEvent TimerUpdated;

	private CountObject countObject;
	private RoundEnd roundEnd;
	private int roundEndCap; // Starting stock / target goals. Unncessary with timer.
	private int minutes;
	private float seconds;
	private MatchEnd matchEndCriteria; // For ending the match
	private int matchEndValue;
	private ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round

	public int minutesLeft;
	public float secondsLeft;
	public bool useTimer = false;
	public int roundNumber;
	public bool suddenDeath = false;

	public List<CompetitorContainer> competitors;
	public List<TeamContainer> teams;

	public void InitializeFromMasterStateData(){
		MasterStateData masterData = MasterStateController.GetTheMasterData ();

		PlayerStateData[] playerStates = PlayerStateController.GetAllStates();

		if (masterData.battleModeBlueprint == null) { // TODO: Debug code
			masterData.SetBattleDefaults ();
		}
		if (!ReceiveBlueprint (masterData.battleModeBlueprint)) {
			throw new UnityException ("CUSTOM: Blueprint not valid, game cannot be loaded.");
		}


		if (masterData.teams) {
			teams = new List<TeamContainer> (MAXNUMBEROFTEAMS);
		} else {
			teams = null;
		}
		competitors = new List<CompetitorContainer> ();
		for (int i = 0; i<playerStates.Length; i++) {
			if (playerStates[i].active) {
				CompetitorContainer newCompCont = new CompetitorContainer (i+1);
				if (countObject == CountObject.Stocks) {
					newCompCont.SetStock (roundEndCap);
				}
				if (masterData.teams) {
					newCompCont.teamIndex = playerStates[i].team;
					if (teams.SingleOrDefault (s => s.teamID == playerStates[i].team) == null) {
						teams.Add (new TeamContainer (playerStates [i].team));
						Debug.Log ("Added team");
					}
				} else {
					newCompCont.teamIndex = -1;
				}
				competitors.Add (newCompCont);
			}
		}
		roundNumber = 1;
		StatsUpdated.Raise ();
	}

	public void DoGoal(GoalData gd){
		if (teams != null) {
			foreach (var playerID in gd.giverPlayerIDs) {
				foreach (CompetitorContainer competitor in competitors) {
					if (competitors.Single(s=>s.playerID == playerID).teamIndex != competitors.Single(s=>s.playerID == gd.goalPlayerID).teamIndex) {
						teams.Single(t=>t.teamID == competitors.Single(s=>s.playerID == playerID).teamIndex).TeamDidAGoal ();
							break;
						}
				}
			}
		}
		CompetitorContainer giver = null;
		foreach (var playerID in gd.giverPlayerIDs) {
			if (playerID != gd.goalPlayerID) {
				giver = competitors.Single (s => s.playerID == playerID);
				giver.DidAGoal ();
			}
		}
		CompetitorContainer receiver = competitors.Single(s=>s.playerID == gd.goalPlayerID);
		receiver.RemoveStock ();
		if (giver != null) {
			if (CheckRoundEndGoals (giver)) {
				EndRound ();
			}
		} else {
			receiver.goalsScored -= 1;
		}
		if (CheckRoundEndElimination (receiver)) {
			EndRound ();
		}
		if (suddenDeath) {
			EndRound ();
		}
		StatsUpdated.Raise ();
	}

	public void AdvanceTime(float deltaTime){
		if (useTimer) {
			if (!suddenDeath) {
				secondsLeft -= deltaTime;
				if (secondsLeft < 0) {
					minutesLeft -= 1;
					secondsLeft += 60;
					if (minutesLeft < 0) {
						if (roundEnd == RoundEnd.Timer) {
							EndRound ();
						}
					}
				}
				TimerUpdated.Raise ();
			}
		}
	}

	public void EndRound(){
		if (useTimer) {
			minutesLeft = minutes;
			secondsLeft = seconds;
		}
		foreach (CompetitorContainer competitor in competitors) {
			switch (countObject) {
			case CountObject.Goals:
				competitor.roundScoreValue = competitor.goalsScored;
				break;
			case CountObject.Stocks:
				if (competitor.roundScoreValue == 0) {
					competitor.roundScoreValue = competitor.stock;
				}
				break;
			default:
				Debug.LogError ("Undefined CountObject: " + countObject);
				break;
			}
			competitor.eliminated = false;
			if (teams != null) {
				teams.Single(t=>t.teamID == competitor.teamIndex).roundScoreValue += competitor.roundScoreValue;
			}
		}
		if (teams != null) {
			ScoreTeams ();
		} else {
			ScoreTheRound ();
		}
		if (suddenDeath) {
			return;
		}
		if (!EndMatchCheck ()) {
			foreach (CompetitorContainer competitor in competitors) {
				competitor.Reset ();
			}
			if (teams != null) {
				foreach (TeamContainer team in teams) {
					team.Reset ();
				}
			}
			StatsUpdated.Raise ();
			EndOfRound.Raise ();
		}
	}

	private void ScoreTheRound(){
		List<int> indexOrder = GenerateIndexOrder(competitors.Cast<ICompetitor>().ToList());
		if (roundEnd == RoundEnd.Timer && countObject == CountObject.Goals && scoringMode != ScoringMode.Direct1to1 &&
		    (competitors [indexOrder [0]].goalsScored == competitors [indexOrder [1]].goalsScored)) {
			suddenDeath = true;
			return;
		}

		for (int i = 0; i < competitors.Count; i++) {
			if (!suddenDeath) {
				switch (scoringMode) {
				case ScoringMode.Direct1to1:
					competitors [i].score += competitors [i].roundScoreValue;
					break;
				case ScoringMode.PerPosition:
					competitors [i].score += (indexOrder.Count - indexOrder.IndexOf (i) - 1);
					Debug.Log ("Competitor " + i + " got " + (indexOrder.Count - indexOrder.IndexOf (i) - 1) + " score ");
					break;
				case ScoringMode.WinnerOnly:
					if (competitors[i].roundScoreValue == competitors[indexOrder[0]].roundScoreValue) {
						competitors [i].score += 1;
					}
					break;
				default:
					Debug.LogError ("No defined scoringMode: " + scoringMode);
					break;
				}
				competitors [i].eliminated = false;
				competitors [i].roundScoreValue = 0;
			} else {
				if (competitors[i].roundScoreValue == competitors[indexOrder[0]].roundScoreValue) {
					competitors [i].score += 1;
				}
			}
		}
		suddenDeath = false;
	}


	private void ScoreTeams(){
		List<int> indexOrder = GenerateIndexOrder (teams.Cast<ICompetitor>().ToList());
		if (roundEnd == RoundEnd.Timer && countObject == CountObject.Goals &&
			(teams [indexOrder [0]].goals == teams [indexOrder [1]].goals)) {
			suddenDeath = true;
			return;
		}
		for (int i = 0; i < teams.Count; i++) {
			if (teams [i] != null) {
				if (!suddenDeath) {
					switch (scoringMode) {
					case ScoringMode.Direct1to1:
						teams [i].score += teams [i].roundScoreValue;
						break;
					case ScoringMode.PerPosition:
						teams [i].score += (indexOrder.Count - indexOrder.IndexOf (i) - 1);
						Debug.Log ("Team " + i + " got " + (indexOrder.Count - indexOrder.IndexOf (i) - 1) + " score ");
						break;
					case ScoringMode.WinnerOnly:
						if (teams[i].roundScoreValue == teams[indexOrder[0]].roundScoreValue) {
							teams [i].score += 1;
						}
						break;
					default:
						Debug.LogError ("No defined scorinMode: " + scoringMode);
						break;
					}
				} else {
					if (teams[i].roundScoreValue == teams[indexOrder[0]].roundScoreValue) {
						teams [i].score += 1;
					}
				}
			}
		}
		suddenDeath = false;
	}

	private List<int> GenerateIndexOrder(List<ICompetitor> compList){
		List<int> indexOrder = new List<int> ();
		bool addLast = true;
		for (int i = 0; i<compList.Count; i++) {
			for (int j = 0; j<indexOrder.Count;j++) {
				if (compList[indexOrder[j]].GetRoundScoreValue() < compList[i].GetRoundScoreValue()) {
					indexOrder.Insert (j, i);
					addLast = false;
					break;
				}
			}
			if (addLast) {
				indexOrder.Add (i);
			} else {
				addLast = true;
			}
		}
		return indexOrder;
	}

	public bool EndMatchCheck(){
		roundNumber += 1;
		switch (matchEndCriteria) {
		case MatchEnd.Rounds:
			if (roundNumber > matchEndValue) {
				return MatchEndCalls ();
			}
			break;
		case MatchEnd.ScoreCap:
			if (teams != null) {
				foreach (TeamContainer team in teams) {
					if (team.score >= matchEndValue) {
						return MatchEndCalls ();
					}
				}
			} else {
				foreach (CompetitorContainer competitor in competitors) {
					if (competitor.score >= matchEndValue) {
						return MatchEndCalls ();
					}
				}
			}
			break;
		default:
			Debug.LogError ("Undefined matchEndCriteria: " + matchEndCriteria);
			break;
		}
		return false;
	}

	private bool MatchEndCalls(){
		bool isMatchSuddenDeath = false;
		if (teams != null) {
			isMatchSuddenDeath = SetTeamOrder ();
		} else {
			isMatchSuddenDeath = SetCompetitorOrder ();
		}
		if (isMatchSuddenDeath) {
			suddenDeath = true;

		} else {
			EndOfMatch.Raise ();
			Debug.Log ("Match has ended");
		}
		return !isMatchSuddenDeath;
	}

	private bool SetCompetitorOrder(){
		foreach (CompetitorContainer competitor in competitors) {
			competitor.roundScoreValue = competitor.score;
		}
		List<int> IndexOrder = GenerateIndexOrder (competitors.Cast<ICompetitor>().ToList());
		for (int i = 0; i < competitors.Count; i++) {
			competitors [i].endPosition = (IndexOrder.IndexOf (i) + 1);
		}
		return (competitors.Single (c => c.endPosition == 1).score == competitors.Single (c => c.endPosition == 2).score);
	}

	private bool SetTeamOrder(){
		foreach (TeamContainer team in teams) {
			team.roundScoreValue = team.score;
		} 
		List<int> IndexOrder = GenerateIndexOrder (teams.Cast<ICompetitor>().ToList());
		for (int i = 0; i < teams.Count; i++) {
			teams [i].endPosition = (IndexOrder.IndexOf (i) + 1);
		}
		return (teams.Single (t => t.endPosition == 1).score == teams.Single (t => t.endPosition == 2).score);
	}

	private bool ReceiveBlueprint(BattleModeBlueprint blueprint){
		if (blueprint == null) {
			Debug.LogWarning ("Received null BattleMode blueprint");
			return false;
		}
		if (blueprint.Validate ()) {
			countObject = blueprint.countObject;
			roundEndCap = blueprint.roundEndCap;
			roundEnd = blueprint.roundEnd;
			if (roundEnd == RoundEnd.Timer) {
				useTimer = true;
				minutes = blueprint.timer.minutes;
				seconds = blueprint.timer.seconds;
				minutesLeft = minutes;
				secondsLeft = seconds;
				TimerUpdated.Raise ();
			} else {
				useTimer = false;
			}
			roundEndCap = blueprint.roundEndCap;
			matchEndCriteria = blueprint.endCriteria;
			matchEndValue = blueprint.endValue;
			scoringMode = blueprint.scoringMode;
			return true;
		} else {
			return false;
		}
	}

	public bool CheckRoundEndGoals(CompetitorContainer goalGiver){
		if (roundEnd == RoundEnd.Cap && countObject == CountObject.Goals) {
			if (teams != null) {
				if (teams [goalGiver.teamIndex].goals >= roundEndCap) {
					return true;
				}
			} else {
				if (goalGiver.goalsScored >= roundEndCap) {
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckRoundEndElimination(CompetitorContainer goalReceiver){
		if (roundEnd == RoundEnd.Elimination && countObject == CountObject.Stocks) {
			if (teams != null) {
				foreach (CompetitorContainer competitor in competitors) {
					if (competitor.teamIndex == goalReceiver.teamIndex) {
						if (competitor.stock > 0) {
							return false;
						}
					}
				}
				return true;
			} else {
				if (goalReceiver.stock <= 0) {
					if (!goalReceiver.eliminated) {
						goalReceiver.eliminated = true;
						// TODO: Eliminate event
						int alive = 0;
						foreach (CompetitorContainer competitor in competitors) {
							if (!competitor.eliminated) {
								alive += 1;
							}
						}
						goalReceiver.score += competitors.Count - alive - 1;
						if (alive <= 1) {
							return true;
						}
					}
				}
			}
		}
		return false;
	}
}

public interface ICompetitor{
	int GetRoundScoreValue();
}

public class TeamContainer : ICompetitor{
	public TeamContainer(int teamID){
		this.teamID = teamID;
		goals = 0;
		score = 0;
		roundScoreValue = 0;
	}

	public int GetRoundScoreValue(){
		return roundScoreValue;
	}

	public void TeamDidAGoal(){
		goals += 1;
	}

	public void AddScore(int amount){
		score += amount;
	}

	public void Reset(){
		goals = 0;
		roundScoreValue = 0;
	}

	public int teamID;
	public int goals;
	public int score;
	public int roundScoreValue;
	public int endPosition;
}

public class CompetitorContainer : ICompetitor{
	public int playerID;
	public int teamIndex;
	public int score;
	public int stock;
	private int maxStock;
	public int goalsScored;
	public bool eliminated;
	public int roundScoreValue;
	public int endPosition;

	public CompetitorContainer(int playerID){
		this.playerID = playerID;
		score = 0;
		stock = 0;
		maxStock = 0;
		goalsScored = 0;
		eliminated = false;
		roundScoreValue = 0;
	}

	public int GetRoundScoreValue(){
		return roundScoreValue;
	}

	public void DidAGoal(){
		goalsScored += 1;
	}

	public void RemoveStock(){
		stock -= 1;
		if (stock < 0) {
			stock = 0;
		}
	}

	public void SetStock(int value){
		Debug.Log (value);
		stock = value;
		maxStock = value;
	}

	public void Reset(){
		stock = maxStock;
		roundScoreValue = 0;
		eliminated = false;
		goalsScored = 0;
	}
}

public class BattleModeBlueprint{
	public CountObject countObject;
	public RoundEnd roundEnd;
	public int roundEndCap; // Starting stock or goalCap, per round. Unnecessary with timer.
	public ATime timer;
	public MatchEnd endCriteria; // For ending the match
	public int endValue;
	public ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round

	public bool Validate(){
		if (endValue < 1) {
			return false;
		}
		if (roundEnd == RoundEnd.Cap && roundEndCap < 1) {
			return false;
		}
		if (roundEnd == RoundEnd.Elimination && roundEndCap < 1) {
			return false;
		}
		if (roundEnd == RoundEnd.Timer && ((timer.minutes <= 0 && timer.seconds <= 0) || timer.seconds >= 60)) {
			return false;
		}
		return true;
	}
	// TODO: Create BattleModeBP validation error event?
}

public class ATime{
	public bool used;
	public int minutes;
	public float seconds;
	public string str;

	public ATime(){
		used = false;
		minutes = 0;
		seconds = 0;
		str = "";
	}

	public ATime(int minutes, float seconds){
		used = true;
		this.minutes = minutes;
		this.seconds = seconds;
		str = "";
	}

	public int GetScale(int secondsPerIndex){
		return (minutes * 60 + (int)seconds) / secondsPerIndex;
	}

	public void SetScale(int scale, int secondsPerIndex){
		minutes = scale * secondsPerIndex / 60;
		seconds = (scale * secondsPerIndex) % 60.0f;
	}

	public string GetAsString(){
		if (str == "") {
			return string.Format ("{0}:{1:F1}", minutes, seconds);
		} else {
			return str;
		}
	}
}
