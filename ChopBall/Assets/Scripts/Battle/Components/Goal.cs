using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {

	public int goalNumber; // Begins from 0
	public GoalEvent goalEvent;
	public SpriteRenderer goalMarker;


	private int goalPlayerID;

    private Vector2 characterSpawnPoint;

	private List<GoalTarget> targets;
    private List<CharacterHandler> charactersInArea;
    private GoalAreaCheck areaCheck;

	public void Initialize(int playerID, Color32 color){
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		goalMarker.enabled = true;
		goalMarker.color = color;
		goalPlayerID = playerID;
        characterSpawnPoint = GetComponentInChildren<CharacterSpawnIndicator>().transform.position;
        charactersInArea = new List<CharacterHandler>(16);
        areaCheck = GetComponentInChildren<GoalAreaCheck>();
	}

	// Dunno if this is good way to do this
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Ball")) {
			Debug.Log ("Collision");
			GoalData gd = new GoalData ();
			Ball ball = collider.gameObject.GetComponent<Ball> ();
			gd.goalPlayerID = goalPlayerID;
			gd.giverPlayerIDs = ball.touchedPlayers;
			gd.goalNumber = goalNumber;
			goalEvent.Raise (gd);
			ball.ResetBallPosition ();
			ResetGoalTargets ();
            EvictCharactersFromArea();
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

    private void EvictCharactersFromArea()
    {
        charactersInArea.Clear();
        charactersInArea = areaCheck.GetCharactersInArea();

        foreach (CharacterHandler c in charactersInArea)
        {
			SetCharPosAndRot (c, Vector2.zero);
        }
    }

	public void SetCharPosAndRot(CharacterHandler handler, Vector2 relativePos){
		handler.SetPositionAndRotation ((Vector2)characterSpawnPoint + relativePos, gameObject.transform.rotation.eulerAngles.z - 90);
	}
}