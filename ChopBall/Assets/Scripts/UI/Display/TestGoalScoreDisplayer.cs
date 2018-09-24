using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGoalScoreDisplayer : MonoBehaviour {

	public List<Text> displayTextList;
	public Text timerDisplay;
	private bool started = false;


	public void StartGame(){
		started = true;
	}

	public void TimerUpdated(){
		ATime timer =  CurrentBattleController.GetATime ();
		timerDisplay.text = timer.minutes + " : " + timer.seconds;
	}

	public void StatsUpdated(){
		int i = 0;
		foreach(CompetitorContainer competitor in CurrentBattleController.GetCompetitors ()){
			displayTextList [i].text = "Goals: " + competitor.goalsScored;
			displayTextList [i].text += "Score: " + competitor.score;
			if (competitor.stock != 0){
				displayTextList [i].text += "Stocks: " + competitor.stock;
			}
			i++;
		}
	}
}
