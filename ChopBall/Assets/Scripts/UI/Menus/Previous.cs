using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Previous : MonoBehaviour
{
	public Previous previousPanel;

	void OnEnable(){
		MenuBack.SetCurrentPanel (this);
	}

}
