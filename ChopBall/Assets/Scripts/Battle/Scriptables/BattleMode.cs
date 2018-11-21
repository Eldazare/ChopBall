using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum CountObject{Lives, Goals}
public enum RoundEnd{Elimination, Cap, Timer}
public enum MatchEnd{Rounds, ScoreCap}
public enum ScoringMode{WinnerOnly, PerPosition, Direct1to1}

[CreateAssetMenu]
public class BattleMode : ScriptableObject {

	int MAXNUMBEROFTEAMS = 4;

	public int numberOfGoals;

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

	public ATime timer;
	public bool useTimer = false;
	public int roundNumber;
	public bool suddenDeath = false;
	public int maxStock = -1;

	public List<CompetitorContainer> competitors;
	public List<TeamContainer> teams;
	public List<GoalInfo> goalDatas;

	public void InitializeFromMasterStateData(int numberOfGoals){
		this.numberOfGoals = numberOfGoals;
		MasterStateData masterData = MasterStateController.GetTheMasterData ();

		PlayerStateData[] playerStates = PlayerStateController.GetAllStates();
		goalDatas = new List<GoalInfo> ();
		if (masterData.battleModeBlueprint == null) { // TODO: Debug code
			masterData.SetBattleDefaults ();
		}
		if (!ReceiveBlueprint (masterData.battleModeBlueprint)) {
			throw new UnityException ("CUSTOM: Blueprint not valid, game cannot be loaded.");
		}
		if (countObject == CountObject.Lives) {
			maxStock = roundEndCap;
		} else {
			maxStock = -1;
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
				if (masterData.teams) {
					newCompCont.teamIndex = playerStates[i].team;
					if (teams.SingleOrDefault (s => s.teamID == playerStates[i].team) == null) {
						TeamContainer tem = new TeamContainer (playerStates [i].team);
						teams.Add (tem);
						Debug.Log ("Added team");
					}
					teams.Single (s => s.teamID == playerStates [i].team).playerIndexes.Add (i);
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
				if (competitors.Single(s=>s.playerID == playerID).teamIndex != competitors.Single(s=>s.playerID == gd.goalPlayerID).teamIndex) {
					teams.Single(t=>t.teamID == competitors.Single(s=>s.playerID == playerID).teamIndex).TeamDidAGoal ();
					break;
				}
			}
		}
		CompetitorContainer giver = null;
		foreach (var playerID in gd.giverPlayerIDs) {
			if (playerID != gd.goalPlayerID) {
				giver = competitors.Single (s => s.playerID == playerID);
				giver.DidAGoal ();
				break;
			}
		}
		CompetitorContainer receiver = competitors.Single(s=>s.playerID == gd.goalPlayerID);
		if (countObject == CountObject.Lives) {
			goalDatas.SingleOrDefault (g => g.goalIndex == gd.goalIndex).stocks -= 1;
		}
		if (giver != null) {
			if (CheckRoundEndGoals (giver)) {
				EndRound ();
			}
		} else {
			receiver.goalsScored -= 1;
		}
		if (CheckRoundEndElimination (gd.goalIndex)) {
			EndRound ();
		}
		if (suddenDeath) {
			EndRound ();
		}
		StatsUpdated.Raise ();
	}

	public void ProgressTime(float deltaTime){
		if (useTimer) {
			if (!suddenDeath) {
				timer.seconds -= deltaTime;
				if (timer.seconds < 0) {
					timer.minutes -= 1;
					timer.seconds += 60;
					if (timer.minutes < 0) {
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
			timer.minutes = minutes;
			timer.seconds = seconds;
		}
		foreach (CompetitorContainer competitor in competitors) {
			switch (countObject) {
			case CountObject.Goals:
				competitor.roundScoreValue = competitor.goalsScored;
				break;
			case CountObject.Lives:
				if(teams == null) {
					if (competitor.roundScoreValue == 0) {
						competitor.roundScoreValue = goalDatas.Single (s => s.goalIndex == competitor.goalIndex).stocks;
					}
					competitor.roundScoreValue += competitor.eliminatedScore;
				}
				break;
			default:
				Debug.LogError ("Undefined CountObject: " + countObject);
				break;
			}
			if (teams != null) {
				teams.Single(t=>t.teamID == competitor.teamIndex).roundScoreValue += competitor.roundScoreValue;
			}
		}

		if (teams != null) {
			if (countObject == CountObject.Lives){
				foreach (var goal in goalDatas) {
					teams.Single (s => s.goalIndexes.Contains (goal.goalIndex)).roundScoreValue += goal.stocks;
				}
			}
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
			foreach (var goal in goalDatas) {
				goal.stocks = maxStock;
			}
			StatsUpdated.Raise ();
			EndOfRound.Raise ();
			Debug.Log ("roundEnd");
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
					//Debug.Log ("Competitor " + i + " got " + (indexOrder.Count - indexOrder.IndexOf (i) - 1) + " score ");
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
				timer.minutes = minutes;
				timer.seconds = seconds;
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

	public bool CheckRoundEndElimination(int goalIndexReduced){
		GoalInfo infoG = null;
		if (roundEnd == RoundEnd.Elimination && countObject == CountObject.Lives) {
			int alive = 0;
			if (teams != null) {
				foreach (TeamContainer team in teams) {
					foreach (var index in team.goalIndexes) {
						if (goalDatas.Single (g => g.goalIndex == index).stocks > 0) {
							infoG = goalDatas.Single (g => g.goalIndex == index);
							alive += 1;
							break;
						}
					}
				}
			} else {
				foreach (var data in goalDatas) {
					if (data.stocks > 0) {
						infoG = data;
						alive += 1;
					}
				}
				if (goalDatas.Single (s => s.goalIndex == goalIndexReduced).stocks <= 0) {
					CompetitorContainer comp = competitors.Single (s => s.goalIndex == goalIndexReduced);
					if (comp.eliminatedScore != 0) {
						comp.eliminatedScore = competitors.Count - alive - 1;
					}
				}
			}
			if (alive <= 1) {
				if (infoG != null) {
					if (teams == null) {
						competitors.Single (s => s.goalIndex == infoG.goalIndex).eliminatedScore = competitors.Count - alive - 1;
					}
				}
				return true;
			}
		}
		return false;
	}

	public bool InitializeGoal(int playerID, int goalIndex){
		goalDatas.Add(new GoalInfo(goalIndex, maxStock));
		if (countObject == CountObject.Lives && competitors.Count > 0) {
			if (teams != null) {
				teams.Single (s => s.teamID == competitors.Single (c => c.playerID == playerID).teamIndex).AddGoalIndex (goalIndex);
			} else {
				competitors.Single (s => s.playerID == playerID).goalIndex = goalIndex;
				Debug.Log ("GoalIndex: " + goalIndex);
			}
		}
		return true;
	}
}

public interface ICompetitor{
	int GetRoundScoreValue();
}

public class TeamContainer : ICompetitor{
	public int teamID;
	public int goals;
	public List<int> goalIndexes;
	public int score;
	public int roundScoreValue;
	public int endPosition;
	public List<int> playerIndexes;

	public void AddGoalIndex(int goalIndex){
		goalIndexes.Add (goalIndex);
	}

	public TeamContainer(int teamID){
		this.teamID = teamID;
		goals = 0;
		score = 0;
		goalIndexes = new List<int> ();
		roundScoreValue = 0;
		playerIndexes = new List<int> ();
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
}

public class CompetitorContainer : ICompetitor{
	public int playerID;
	public int teamIndex;
	public int score;
	public int goalsScored;
	public int goalIndex;
	public int roundScoreValue;
	public int endPosition;
	public ATime eliminatedTime;
	public int eliminatedScore;

	public CompetitorContainer(int playerID){
		this.playerID = playerID;
		score = 0;
		goalsScored = 0;
		roundScoreValue = 0;
		eliminatedScore = 0;
		goalIndex = -1;
	}

	public int GetRoundScoreValue(){
		return roundScoreValue;
	}

	public void DidAGoal(){
		goalsScored += 1;
	}

	public void Reset(){
		roundScoreValue = 0;
		goalsScored = 0;
		eliminatedScore = 0;
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

public class GoalInfo{
	public int goalIndex;
	public int stocks;

	public GoalInfo(int goalIndex, int stocks){
		this.goalIndex = goalIndex;
		this.stocks = stocks;
	}
}
