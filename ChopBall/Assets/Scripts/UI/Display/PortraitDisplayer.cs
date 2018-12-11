using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PortraitDisplayer : MonoBehaviour {

	public List<Image> portraitList;
	private int lastGoal = 0;
	private PortraitBaseData pbd;
	private List<int> IDList;
	private bool teams;

	List<CompetitorContainer> competitors;

	public void GameStart(){
		teams = CurrentBattleController.GetTeams () != null;
		pbd = (PortraitBaseData) Resources.Load ("Scriptables/_BaseDatas/PortraitBaseData");
		competitors = CurrentBattleController.GetCompetitors ();
		Reset (true);
		Debug.Log ("DONE!");
	}

	void Reset(bool AddIDs = false){
		if (AddIDs) {
			IDList = new List<int> ();
		}
		int i = 0;
		Debug.Log (competitors.Count);
		foreach (var competitor in competitors) {
			if (teams) {
				portraitList [i].sprite = pbd.neutralList [competitor.teamIndex];
			} else {
				portraitList [i].sprite = pbd.neutralList [competitor.playerID - 1];
			}
			if (AddIDs) {
				IDList.Add (competitor.playerID);
			}
			i++;
		}
	}

	void OnDisable(){
		this.StopAllCoroutines ();
	}

	void Update(){
		
	}


	public void GetGoalData(GoalData data){
		int giver = data.GetTrueGiver ();
		if (teams) {
			giver = competitors.Single (c => c.playerID == giver).teamIndex;
		}
		if (lastGoal == giver) {
			portraitList [IDList.IndexOf (giver)].sprite = pbd.inFlamesList [giver - 1];
			return;
		} else {
			Reset (false);
		}
		StartCoroutine (ChangeSprite (giver));
	}

	private IEnumerator ChangeSprite(int giver){
		portraitList [IDList.IndexOf(giver)].sprite = pbd.successList [giver - 1];
		yield return new WaitForSeconds (3.0f);
		portraitList [IDList.IndexOf(giver)].sprite = pbd.neutralList [giver - 1];
		lastGoal = giver;
	}
}
