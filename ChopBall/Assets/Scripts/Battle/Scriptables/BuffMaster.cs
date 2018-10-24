using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuffMaster : ScriptableObject {

	List<_Buff> activeBuffs;


	public void Initialize(){
		activeBuffs = new List<_Buff> ();
	}

	public void ReduceTime(float deltaTime){
		for(int i = activeBuffs.Count - 1; i >= 0; i--) {
			if (activeBuffs[i].ReduceTime (deltaTime)) {
				activeBuffs.RemoveAt(i);
			}
		}
	}

	public void EndAllBuffs(){
		foreach (var buff in activeBuffs) {
			buff.Die();
		}
		activeBuffs.Clear ();
	}

	public void AddBuff(_Buff buff){
		buff.ModifyMods (true);
		activeBuffs.Add (buff);
	}

	public void EndBuff(_Buff buff){
		if (activeBuffs.Contains (buff)) {
			activeBuffs.Remove (buff);
		} else {
			Debug.LogError ("Didn't find buff to remove");
		}
	}
}
