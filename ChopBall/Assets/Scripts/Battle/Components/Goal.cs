using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	public GoalEvent goalEvent;

	public int goalPlayerID;
	public int goalTeamID;

	// Dunno if this is good way to do this
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Ball")) {
			Debug.Log ("Collision");
			GoalData gd = new GoalData ();
			Ball ball = collider.gameObject.GetComponent<Ball> ();
			gd.goalPlayerID = goalPlayerID;
			gd.teamGoalID = goalTeamID;
			gd.giverPlayerID = ball.lastTouchedPlayerID;
			goalEvent.Raise (gd);
			ball.ResetBallPosition ();
		}
	}
}
