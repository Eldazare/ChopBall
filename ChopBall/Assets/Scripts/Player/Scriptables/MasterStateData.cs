using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageChoiceType {individualRandom, masterSingle, randomPreset}

[CreateAssetMenu]
public class MasterStateData : ScriptableObject {
	// Runtime "Options" that reset 

	// TODO: Presets in the UI component. Might be stored as scriptable themselves.


	public int numberOfPlayers; // TODO: Define how this is read / input by player?
	public bool teams; // Wether the game is Free-For-All or Teams
	public StageChoiceType stageChoiceType;
	//public GameMode gameModeChoice;

	// stocksTimer variables
	public ATime stocksTimerTimer;
	public int stocksTimerStocks;

	public ScoringMode stocksTimerScoringMode;
	public MatchEnd stocksTimerEndEnum;
	public int stocksTimerEndValue;

	// stocksElim variables
	public int stocksElimStocks;
	public int stocksElimScoreLimit;

	public ScoringMode stocksElimScoringMode;
	public MatchEnd stocksElimEndEnum;
	public int stocksElimEndValue;

	// goalsTimer
	public int goalsTimerGoals;

	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;

	/*
	public void SetDefaults(){
		// Should contain every field
		numberOfPlayers = 1;
		teams = false;
		stageChoiceType = StageChoiceType.individualRandom;
		timer = new ATime (8, 0f);
		stocks = 0; // infinite (or not used)
		goalLimit = 10;
		stageNameFinal = "";
		totalRounds = 1;
		roundWins = 0;
	}
	*/
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
