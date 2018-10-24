using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {

	public GoalEvent goalEvent;
	public SpriteRenderer goalMarker;


	public int goalPlayerID;

	private List<GoalTarget> targets;

	public void Initialize(int playerID, Color32 color){
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		goalMarker.enabled = true;
		goalMarker.color = color;
		goalPlayerID = playerID;
	}

	// Dunno if this is good way to do this
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Ball")) {
			Debug.Log ("Collision");
			GoalData gd = new GoalData ();
			Ball ball = collider.gameObject.GetComponent<Ball> ();
			gd.goalPlayerID = goalPlayerID;
			gd.giverPlayerIDs = ball.touchedPlayers;
			goalEvent.Raise (gd);
			ball.ResetBallPosition ();
			ResetGoalTargets ();
		}
	}

	public void AddTargetToGoal(GoalTarget target){
		if (targets == null) {
			targets = new List<GoalTarget> ();
		}
		targets.Add (target);
	}

	private void ResetGoalTargets(){
		foreach(var target in targets){
			target.Activate ();
		}
	}
}
