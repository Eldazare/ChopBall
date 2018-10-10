using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneDisplay : MonoBehaviour {

	public List<Text> textList;

	private List<Text> displayTextList;

	void Awake(){
		List<CompetitorContainer> competitors = CurrentBattleController.GetCompetitors ();
		List<TeamContainer> teams = CurrentBattleController.GetTeams ();
		displayTextList = new List<Text> ();
		if (teams != null) {
			Debug.LogWarning ("Unimplemented");
		} else {
			PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load ("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
			for (int i = 0; i<competitors.Count; i++) {
				displayTextList.Add (textList [i]);
				displayTextList [i].gameObject.SetActive (true);
				displayTextList [i].color = pBaseData.playerColors[competitors[i].playerID-1];
				displayTextList [i].text = "Player "+competitors[i].playerID+":"+"\n"
					+"Position: "+competitors[i].endPosition+"\n"
					+"Score: "+competitors[i].score+"\n";
			}
			UpdatePositions ();
		}
	}

	public void UpdatePositions(){
		if (displayTextList != null){
			//Debug.Log ("Height :" + Screen.height + " Width: " + Screen.width);
			float yPos = 0f;
			float relativeX = Screen.width / displayTextList.Count;
			float xPos = Screen.width * -0.5f+ relativeX / 2.0f;
			for (int i = 0; i < displayTextList.Count; i++) {
				displayTextList [i].GetComponent<RectTransform> ().anchoredPosition = new Vector2 (xPos, yPos);
				xPos += relativeX;
			}
		}
	}
}
