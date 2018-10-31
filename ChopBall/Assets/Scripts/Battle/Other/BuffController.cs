using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class BuffController {

	private static BuffMaster[] buffMasters;

	private static void LoadInitialize(){
		if (buffMasters == null) {
			buffMasters = Resources.LoadAll ("Scriptables/Players/BuffMasters/").Cast<BuffMaster> ().ToArray ();
			if (buffMasters != null) {
				foreach (var buffMaster in buffMasters) {
					buffMaster.Initialize ();
				}
			} else {
				Debug.LogError ("BuffMaster loading failer.");
			}
		}
	}

	public static void AddBuff(int playerID, _Buff buff){
		LoadInitialize ();
		if (buff.magnitude > 0) {
			buff.GiveMods (RuntimeModifierController.GetAMod (playerID));
			buffMasters [playerID - 1].AddBuff (buff);
		} else {
			Debug.LogError ("Buff with magnitude 0 (or less) given");
		}
	}

	public static void EndBuff(int playerID, _Buff buff){
		LoadInitialize ();
		buffMasters [playerID - 1].EndBuff (buff);
	}

	public static void EndAllBuffs(){
		LoadInitialize ();
		foreach (var buffMaster in buffMasters) {
			buffMaster.EndAllBuffs ();
		}
	}

	public static void ProgressTime(float deltaTime){
		LoadInitialize ();
		foreach (var buffMaster in buffMasters) {
			buffMaster.ReduceTime (deltaTime);
		}
	}
}
