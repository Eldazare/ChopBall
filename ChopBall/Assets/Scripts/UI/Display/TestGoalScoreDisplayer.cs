using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGoalScoreDisplayer : MonoBehaviour {

	public List<Text> displayTextList;
	public Text timerDisplay;

	public void TimerUpdated(){
		ATime timer =  CurrentBattleController.GetATime ();
		timerDisplay.text = string.Format ("{0}:{1:F1}", timer.minutes, timer.seconds);
	}

	public void StatsUpdated(){
		int i = 0;
		foreach(CompetitorContainer competitor in CurrentBattleController.GetCompetitors ()){
			if (competitor != null) {
				displayTextList [i].text = "Goals: " + competitor.goalsScored;
				displayTextList [i].text += "\nScore: " + competitor.score;
				if (competitor.stock != 0) {
					displayTextList [i].text += "\nStocks: " + competitor.stock;
				}
			}
			i++;
		}
	}
}
