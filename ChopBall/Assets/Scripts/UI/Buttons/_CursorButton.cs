using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class _CursorButton : MonoBehaviour {

	public abstract void Click(int playerID);
	public abstract void Hover(int playerID);
	public abstract void OnHoverExit(int playerID);
}
