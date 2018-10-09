using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndSceneDisplay : MonoBehaviour {

	public List<Text> textList;

	void Awake(){
		List<CompetitorContainer> competitors = CurrentBattleController.GetCompetitors ();
		List<TeamContainer> teams = CurrentBattleController.GetTeams ();
		if (teams != null) {
			Debug.LogWarning ("Unimplemented");
		} else {
			for (int i = 0; i<competitors.Count; i++) {
				textList [i].text = "Player "+competitors[i].playerID+":"+"\n"
					+"Position: "+competitors[i].endPosition+"\n"
					+"Score: "+competitors[i].score+"\n";
			}
		}
	}
}
