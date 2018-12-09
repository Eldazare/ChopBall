using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerBaseData : ScriptableObject {

	[Header("General")]
	public GameEvent OnCharacterChosen;
	public GameEvent OnTeamChanged;
	public List<Color32> playerColors; // PlayerID = (Index+1)
	public List<Color32> teamColors; // Max teams = 4?

	[Header("CharChoose BG sprites")]
	public List<Sprite> P1BGs;
	public List<Sprite> P2BGs;
	public List<Sprite> P3BGs;
	public List<Sprite> P4BGs;




	public List<Sprite> GetBGSprites(int playerID){
		switch (playerID) {
		case 1:
			return P1BGs;
		case 2:
			return P2BGs;
		case 3:
			return P3BGs;
		case 4:
			return P4BGs;
		default:
			Debug.LogError ("Invalid PlayerID given to PlayerBaseData");
			return null;
		}
	}
}
