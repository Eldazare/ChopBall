using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGoalScoreDisplayer : MonoBehaviour {

	public List<Text> allTextList;
	public Text timerDisplay;

	private List<Text> displayTextList;

	public void TimerUpdated(){
		ATime timer =  CurrentBattleController.GetATime ();
		timerDisplay.text = string.Format ("{0}:{1:F1}", timer.minutes, timer.seconds);
	}

	public void ResolutionChangeUpdatePositions(){
		if (displayTextList != null){
			Debug.Log ("Height :" + Screen.height + " Width: " + Screen.width);
			float yPos = (float)Screen.height * -0.475f;
			float relativeX = Screen.width / displayTextList.Count;
			float xPos = Screen.width * -0.5f+ relativeX / 2.0f;
			for (int i = 0; i < displayTextList.Count; i++) {
				displayTextList [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
				xPos += relativeX;
			}
		}
	}

	public void StatsUpdated(){
		if (CurrentBattleController.GetTeams () != null) {
		
		} else {
			if (displayTextList == null) {
				displayTextList = new List<Text> ();
				int j = 0;
				foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors()) {
					displayTextList.Add (allTextList [j]);
					displayTextList [j].gameObject.SetActive (true);
					j++;
				}
				ResolutionChangeUpdatePositions ();
			}
			int i = 0;
			foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors ()) {
				displayTextList [i].text = "Player " + competitor.playerID;
				displayTextList [i].text += "\nGoals: " + competitor.goalsScored;
				displayTextList [i].text += "\nScore: " + competitor.score;
				if (competitor.stock != 0) {
					displayTextList [i].text += "\nStocks: " + competitor.stock;
				}
				i++;
			}
		}
	}
}
