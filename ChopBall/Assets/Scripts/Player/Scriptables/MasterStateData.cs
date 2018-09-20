using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageChoiceType {individualRandom, masterSingle, randomPreset}

[CreateAssetMenu]
public class MasterStateData : ScriptableObject {
	// Runtime "Options" that reset 

	public int numberOfPlayers; // TODO: Define how this is read / input by player?
	public bool teams; // Wether the game is Free-For-All or Teams
	public StageChoiceType stageChoiceType;
	public ATime timer;
	public int goalLimit; // 1-N
	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;


	public void SetDefaults(){
		// Should contain every field
		numberOfPlayers = 1;
		teams = false;
		stageChoiceType = StageChoiceType.individualRandom;
		timer = new ATime (8, 0f);
		goalLimit = 10;
		stageNameFinal = "";
	}
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
