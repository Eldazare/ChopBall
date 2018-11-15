using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {

	public GoalEvent goalEvent;
	public SpriteRenderer goalMarker;
	public MeshRenderer goalMarker3D;


	private int goalPlayerID;
	private int goalIndex;

    private Vector2 characterSpawnPoint;

	private List<GoalTarget> targets;
    private List<CharacterHandler> charactersInArea;
    private GoalAreaCheck areaCheck;

	private string soundGoalPath;

	public void Initialize( Color32 color, Material mat, int goalIndex){
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		if (goalMarker != null) {
			goalMarker.enabled = true;
			goalMarker.color = color;
		} if (goalMarker3D != null) {
			goalMarker3D.material = mat;
		}
        charactersInArea = new List<CharacterHandler>(16);
        areaCheck = GetComponentInChildren<GoalAreaCheck>();
		soundGoalPath = SoundPathController.GetPath ("Goal1");
		this.goalIndex = goalIndex;
		CurrentBattleController.InitializeGoalData (goalPlayerID, goalIndex);
	}

	public void InitializeID(int playerID){
		characterSpawnPoint = GetComponentInChildren<CharacterSpawnIndicator>().transform.position;
		this.goalPlayerID = playerID;
	}

	// Dunno if this is good way to do this
	void OnTriggerEnter2D(Collider2D collider){
		if (collider.gameObject.CompareTag ("Ball")) {
			Debug.Log ("Collision");
			GoalData gd = new GoalData ();
			Ball ball = collider.gameObject.GetComponent<Ball> ();
			gd.goalPlayerID = goalPlayerID;
			gd.giverPlayerIDs = ball.touchedPlayers;
			gd.goalIndex = goalIndex;
			goalEvent.Raise (gd);
			ball.ResetBallPosition ();
			ResetGoalTargets ();
            EvictCharactersFromArea();
			FMODUnity.RuntimeManager.PlayOneShot (soundGoalPath, gameObject.transform.position);
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