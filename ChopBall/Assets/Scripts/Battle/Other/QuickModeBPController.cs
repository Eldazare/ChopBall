using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class QuickModeBPController  {

	private static QuickModeBlueprint[] bpList;

	private static void LoadBpList(){
		if (bpList == null) {
			bpList = Resources.LoadAll ("Scriptables/Battle/QuickModes", typeof(QuickModeBlueprint)).Cast<QuickModeBlueprint>().ToArray();
		}
	}


	public static string GetNamePerIndex(int index){
		LoadBpList ();
		return bpList [index].GetName ();
	}

	public static string GetDescriptionPerIndex (int index){
		LoadBpList ();
		return bpList [index].GetDescription ();
	}

	public static QuickModeBlueprint[] GetAllBPs(){
		LoadBpList ();
		return bpList;
	}
}
