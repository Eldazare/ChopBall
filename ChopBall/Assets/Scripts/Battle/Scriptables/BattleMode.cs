using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CountObject{Stocks, Goals}
public enum RoundEnd{Elimination, Cap, Timer}
public enum MatchEnd{Rounds, RoundsWin, ScoreCap}
public enum ScoringMode{WinnerOnly, PerPosition, Direct1to1}

[CreateAssetMenu]
public class BattleMode : ScriptableObject {

	public GameEvent EndOfRound;
	public GameEvent EndOfMatch;

	public CountObject countObject;
	public int countValue; // Starting stock or goalCap, per round
	public RoundEnd roundEnd;
	public int roundEndCap;
	public int minutes;
	public float seconds;
	public MatchEnd endCriteria; // For ending the match
	public int endValue;
	public ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round

	public int minutesLeft;
	public float secondsLeft;
	public bool useTimer = false;

	public void DoGoal(GoalData gd){
		
	}

	public void AdvanceTime(float deltaTime){
		if (useTimer) {
			secondsLeft -= deltaTime;
			if (secondsLeft < 0) {
				minutesLeft -= 1;
				secondsLeft += 60;
				if (minutesLeft < 0) {
					
				}
			}
		}
	}

	public void EndRound(){
		
	}

	public void EndMatch(){
		
	}
}

public class BattleModeBlueprint{
	public CountObject countObject;
	public int countValue; // Starting stock or goalCap, per round
	public RoundEnd roundEnd;
	public int roundEndCap;
	public int minutes;
	public float seconds;
	public MatchEnd endCriteria; // For ending the match
	public int endValue;
	public ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round

	public bool Validate(){
		if (countValue < 1) {
			return false;
		}
		if (endValue < 1) {
			return false;
		}
		if (roundEnd == RoundEnd.Cap && roundEndCap < 1) {
			return false;
		}
		if (roundEnd == RoundEnd.Timer && minutes < 1) {
			return false;
		}
		return true;
	}

	// TODO: Create BattleModeBP validation error event?
}
