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
		timerDisplay.text = timer.GetAsString ();
	}

	public void ResolutionChangeUpdatePositions(){
		if (displayTextList != null){
			if (displayTextList.Count == 0) {
				return;
			}
			int height = 1080;
			int width = 1920;
			Debug.Log ("Height :" + Screen.height + " Width: " + Screen.width);

			float yPos = (float)height * -0.5f+120f;
			float relativeX = (float)width / displayTextList.Count;
			float xPos = (float)width * -0.5f + relativeX / 2.0f;
			for (int i = 0; i < displayTextList.Count; i++) {
				Debug.Log (xPos + "   " + yPos);
				displayTextList [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
				xPos += relativeX;
			}
		}
	}

	public void StatsUpdated(){
		if (CurrentBattleController.GetTeams () != null) {
			if (displayTextList == null) {
				displayTextList = new List<Text> ();
				PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
				int j = 0;
				foreach (TeamContainer team in CurrentBattleController.GetTeams()) {
					displayTextList.Add (allTextList [j]);
					displayTextList [j].gameObject.SetActive (true);
					displayTextList [j].color = pBaseData.teamColors[team.teamID];
					j++;
				}
				ResolutionChangeUpdatePositions ();
			}

			int i = 0;
			foreach (TeamContainer team in CurrentBattleController.GetTeams()) {
				displayTextList [i].text = "Team " + team.teamID;
				displayTextList [i].text += "\nGoals: " + team.goals;
				displayTextList [i].text += "\nScore: " + team.score;
				i++;
			}

		} else {
			if (displayTextList == null) {
				displayTextList = new List<Text> ();
				PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
				int j = 0;
				foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors()) {
					displayTextList.Add (allTextList [j]);
					displayTextList [j].gameObject.SetActive (true);
					displayTextList [j].color = pBaseData.playerColors[competitor.playerID-1];
					j++;
				}
				ResolutionChangeUpdatePositions ();
			}

			int i = 0;
			foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors ()) {
				displayTextList [i].text = "Player " + competitor.playerID;
				displayTextList [i].text += "\nGoals: " + competitor.goalsScored;
				if (competitor.stock != 0) {
					displayTextList [i].text += "\nStocks: " + competitor.stock;
				}
				displayTextList [i].text += "\nScore: " + competitor.score;
				i++;
			}
		}
	}
}
