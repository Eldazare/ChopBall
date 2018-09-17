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
	public float timeLimit; // 0 is infinite
	public int goalLimit; // 1-N
	public string stageNameFinal; // Either from master cursor or voter class.

	public List<string> randomStagePreset;


	public void SetDefaults(){
		// Should contain every field
		numberOfPlayers = 1;
		teams = false;
		stageChoiceType = StageChoiceType.individualRandom;
		timeLimit = 8.00f;
		goalLimit = 10;
		stageNameFinal = "";
	}
}
