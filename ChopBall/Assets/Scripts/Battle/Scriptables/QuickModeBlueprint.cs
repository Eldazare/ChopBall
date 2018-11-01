using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class QuickModeBlueprint : ScriptableObject {

	[Header("Metadata")]
	public string bpName;
	public string descriptionLine1;
	public string descriptionLine2;
	public string descriptionLine3;
	public string descriptionLine4;
	public string descriptionLine5;


	[Header("Blueprint")]
	public CountObject countObject;
	public RoundEnd roundEnd;
	public int roundEndCap; // Starting stock or goalCap, per round. Unnecessary with timer.
	public int timerMinutes;
	public float timerSeconds;
	public MatchEnd endCriteria; // For ending the match
	public int endValue;
	public ScoringMode scoringMode; // How do goals/Stocks/etc relate to score at end of round


	public string GetDescription(){
		return descriptionLine1 + "\n" +
		descriptionLine2 + "\n" +
		descriptionLine3 + "\n" +
		descriptionLine4 + "\n" +
		descriptionLine5;
	}

	public string GetName(){
		return bpName;
	}


	public BattleModeBlueprint GetBlueprint(){
		BattleModeBlueprint bp = new BattleModeBlueprint ();
		bp.countObject = countObject;
		bp.roundEnd = roundEnd;
		bp.roundEndCap = roundEndCap;
		bp.timer = new ATime (timerMinutes, timerSeconds);
		bp.endCriteria = endCriteria;
		bp.endValue = endValue;
		bp.scoringMode = scoringMode;
		return bp;
	}
}
