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

	//
	int MAXNUMBEROFTEAMS = 4;
	//

	public GameEvent EndOfRound;
	public GameEvent EndOfMatch;

	public GameEvent StatsUpdated;
	public GameEvent TimerUpdated;

	private CountObject countObject;
	private RoundEnd roundEnd;
	private int roundEndCap; // Starting stock / target goals. Unncesssary with timer.
	private int minutes;
	private float seconds;
	private MatchEnd matchEndCriteria; // For ending the match
	private int matchEndValue;
	private ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round

	public int minutesLeft;
	public float secondsLeft;
	public bool useTimer = false;
	public int roundNumber;

	public List<CompetitorContainer> competitors;
	public List<TeamContainer> teams;

	public void InitializeFromMasterStateData(){
		MasterStateData masterData = MasterStateController.GetTheMasterData ();

		PlayerStateData[] playerStates = PlayerStateController.GetAllStates();
		if (masterData.teams) {
			teams = new List<TeamContainer> ();
			for (int i = 0; i<MAXNUMBEROFTEAMS;i++) {
				teams.Add (null);
			}
		} else {
			teams = null;
		}
		competitors = new List<CompetitorContainer> ();
		foreach (PlayerStateData stateData in playerStates) {
			if (stateData.active) {
				CompetitorContainer newCompCont = new CompetitorContainer ();
				newCompCont.score = 0;
				newCompCont.goalsScored = 0;
				if (countObject == CountObject.Stocks) {
					newCompCont.SetStock (roundEndCap);
				}
				if (masterData.teams) {
					newCompCont.teamIndex = stateData.team;
					teams [stateData.team] = new TeamContainer ();
				} else {
					newCompCont.teamIndex = -1;
				}
				competitors.Add (newCompCont);
			} else {
				competitors.Add (null);
			}
		}
		roundNumber = 1;
		if (masterData.battleModeBlueprint == null) { // TODO: Debug code
			masterData.SetDefaults ();
		}
		if (!ReceiveBlueprint (masterData.battleModeBlueprint)) {
			throw new UnityException ("CUSTOM: Blueprint not valid, game cannot be loaded.");
		}
	}

	public void DoGoal(GoalData gd){
		if (teams != null) {
			foreach (var playerID in gd.giverPlayerIDs) {
				if (competitors [playerID - 1].teamIndex != competitors [gd.goalPlayerID-1].teamIndex) {
					teams [competitors [playerID - 1].teamIndex].TeamDidAGoal ();
					break;
				}
			}
			competitors [gd.goalPlayerID - 1].goalsScored -= 1;
		}
		CompetitorContainer giver = null;
		foreach (var playerID in gd.giverPlayerIDs) {
			if (playerID != gd.goalPlayerID) {
				giver = competitors [playerID - 1];
				giver.DidAGoal ();
			}
		}
		CompetitorContainer receiver = competitors [gd.goalPlayerID - 1];
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
		StatsUpdated.Raise ();
	}

	public void AdvanceTime(float deltaTime){
		if (useTimer) {
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
				if (competitor.roundScoreValue != 0) {
					competitor.roundScoreValue = competitor.stock;
				}
				break;
			default:
				Debug.LogError ("Undefined CountObject: " + countObject);
				break;
			}
			competitor.eliminated = false;
			if (teams != null) {
				teams [competitor.teamIndex].roundScoreValue += competitor.roundScoreValue;
			}
		}
		if (teams != null) {
			ScoreTeams ();
		} else {
			ScoreTheRound ();
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

		for (int i = 0; i<competitors.Count;i++) {
			switch (scoringMode) {
			case ScoringMode.Direct1to1:
				competitors[i].score += competitors[i].roundScoreValue;
				break;
			case ScoringMode.PerPosition:
				competitors [i].score += (indexOrder.Count - indexOrder.IndexOf(i) - 1);
				Debug.Log("Competitor "+i+" got " + (indexOrder.Count - indexOrder.IndexOf(i) - 1) + " score ");
				break;
			case ScoringMode.WinnerOnly:
				if (indexOrder.IndexOf (i) == 0) {
					competitors [i].score += 1;
				}
				break;
			default:
				Debug.LogError ("No defined scoringMode: " + scoringMode);
				break;
			}
			competitors[i].eliminated = false;
			competitors[i].roundScoreValue = 0;
		}
	}


	private void ScoreTeams(){
		List<int> indexOrder = GenerateIndexOrder (teams.Cast<ICompetitor>().ToList());
		for (int i = 0; i < teams.Count; i++) {
			if (teams [i] != null) {
				switch (scoringMode) {
				case ScoringMode.Direct1to1:
					teams [i].score += teams [i].roundScoreValue;
					break;
				case ScoringMode.PerPosition:
					teams [i].score += (indexOrder.Count - indexOrder.IndexOf(i) - 1);
					Debug.Log ("Team " + i + " got " + (indexOrder.Count - indexOrder.IndexOf(i) - 1) + " score ");
					break;
				case ScoringMode.WinnerOnly:
					if (indexOrder.IndexOf (i) == 0) {
						teams [i].score += 1;
					}
					break;
				default:
					Debug.LogError ("No defined scorinMode: " + scoringMode);
					break;
				}
			}
		}
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
		if (teams != null) {
			SetTeamOrder ();
		} else {
			SetCompetitorOrder ();
		}
		EndOfMatch.Raise ();
		Debug.Log ("Match has ended");
		return true;
	}

	private void SetCompetitorOrder(){
		foreach (CompetitorContainer competitor in competitors) {
			competitor.roundScoreValue = competitor.score;
		}
		List<int> IndexOrder = GenerateIndexOrder (competitors.Cast<ICompetitor>().ToList());
		for (int i = 0; i < competitors.Count; i++) {
			competitors [i].endPosition = (IndexOrder.IndexOf (i) + 1);
		}
	}

	private void SetTeamOrder(){
		foreach (TeamContainer team in teams) {
			team.roundScoreValue = team.score;
		}
		List<int> IndexOrder = GenerateIndexOrder (teams.Cast<ICompetitor>().ToList());
		for (int i = 0; i < teams.Count; i++) {
			teams [i].endPosition = (IndexOrder.IndexOf (i) + 1);
		}
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
				roundEndCap = blueprint.roundEndCap;
			}
			matchEndCriteria = blueprint.endCriteria;
			matchEndValue = blueprint.endValue;
			scoringMode = blueprint.scoringMode;
			StatsUpdated.Raise ();
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
	public TeamContainer(){
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
	public int teamIndex;
	public int score;
	public int stock;
	private int maxStock;
	public int goalsScored;
	public bool eliminated;
	public int roundScoreValue;
	public int endPosition;

	public CompetitorContainer(){
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

	public ATime(){
		used = false;
		minutes = 0;
		seconds = 0;
	}

	public ATime(int minutes, float seconds){
		used = true;
		this.minutes = minutes;
		this.seconds = seconds;
	}
}
