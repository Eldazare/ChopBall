using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitDisplayer : MonoBehaviour {

	public List<Image> portraitList;
	private int lastGoal = 0;
	private PortraitBaseData pbd;
	private List<int> IDList;

	void Start(){
		pbd = (PortraitBaseData) Resources.Load ("Scriptables/_BaseDatas/PortraitBaseData");
		IDList = new List<int> ();
		Reset (true);
	}

	void Reset(bool AddIDs = false){
		int i = 0;
		foreach (var competitor in CurrentBattleController.GetCompetitors()) {
			portraitList [i].sprite = pbd.neutralList [competitor.playerID-1];
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
