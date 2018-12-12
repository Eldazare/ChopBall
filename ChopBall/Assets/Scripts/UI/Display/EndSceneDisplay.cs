using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneDisplay : MonoBehaviour {

	public Text winnerText;
	public List<Text> textList;
	private int[] fontScales = { 90,75,65,65 };

	private List<Text> displayTextList;

	void Awake(){
		List<CompetitorContainer> competitors = CurrentBattleController.GetCompetitors ();
		List<TeamContainer> teams = CurrentBattleController.GetTeams ();
		displayTextList = new List<Text> ();
		PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
		if (teams != null) {
			for (int i = 0; i < teams.Count; i++) {
				displayTextList.Add (textList [i]);
				displayTextList [i].gameObject.SetActive (true);
				displayTextList [i].color = pBaseData.teamColors [teams [i].teamID];
				displayTextList [i].text = "Team " + (teams [i].teamID+1) + "\n"
				+ "No. " + teams [i].endPosition + "\n"
				+ "Score: " + teams [i].score + "\n";
				if (teams [i].endPosition != 0) {
					displayTextList [i].fontSize = fontScales [teams [i].endPosition - 1];
				}
				if (teams [i].endPosition == 1) {
					winnerText.color = pBaseData.teamColors [teams [i].teamID];
					winnerText.text = "Team " + (teams [i].teamID+1) + " is the winner!";
				}
			}
		} else if (competitors != null) {
			for (int i = 0; i < competitors.Count; i++) {
				displayTextList.Add (textList [i]);
				displayTextList [i].gameObject.SetActive (true);
				displayTextList [i].color = pBaseData.playerColors [competitors [i].playerID - 1];
				displayTextList [i].text = "Player " + competitors [i].playerID + "\n"
				+ "No. " + competitors [i].endPosition + "\n"
				+ "Score: " + competitors [i].score + "\n";
				if (competitors [i].endPosition != 0) {
					displayTextList [i].fontSize = fontScales [competitors [i].endPosition - 1];
				}
				if (competitors [i].endPosition == 1) {
					winnerText.color = pBaseData.playerColors [competitors [i].playerID - 1];
					winnerText.text = "Player " + competitors [i].playerID + " is the winner!";
				}
			}
		} else {
			Debug.LogWarning ("No displaydata found.");
		}
		UpdatePositions ();
	}

	public void UpdatePositions(){
		if (displayTextList != null){
			if (displayTextList.Count > 0) {
				//Debug.Log ("Height :" + Screen.height + " Width: " + Screen.width);
				int width = 1920;
				//int height = 1080;
				float yPos = 0f;
				float relativeX = width / displayTextList.Count;
				float xPos = width * -0.5f + relativeX / 2.0f;
				for (int i = 0; i < displayTextList.Count; i++) {
					displayTextList [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
					xPos += relativeX;
				}
			}
		}
	}
}
