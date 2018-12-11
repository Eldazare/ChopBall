using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalData{
	public List<int> giverPlayerIDs;
	public int goalPlayerID;
	public int goalIndex;


	public int GetTrueGiver(){
		foreach (var playerID in giverPlayerIDs) {
			if (playerID != goalPlayerID) {
				return playerID;
			}
		}
		return -1;
	}
}
