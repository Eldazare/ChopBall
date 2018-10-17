using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerBaseData : ScriptableObject {

	public GameEvent OnCharacterChosen;
	public GameEvent OnTeamChanged;
	public List<Color32> playerColors; // PlayerID = (Index+1)
	public List<Color32> teamColors; // Max teams = 4?
}
