using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticleHelper : MonoBehaviour {

	public List<ParticleSystem> particleSystems;

	public void GetGoalData(GoalData data){
		particleSystems [data.goalIndex].Play ();
	}
}
