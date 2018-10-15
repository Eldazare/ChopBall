using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ALSO handles proceeding from character select
public static class StageTagHandler {

	private static List<List<StageTag>> tagList;

	private static void InitializeTagList(){
		tagList = new List<List<StageTag>> (2);
		tagList.Add(new List<StageTag> (){StageTag.T1v1, StageTag.FFA3, StageTag.FFA4 });
		tagList.Add(new List<StageTag> (){ StageTag.T1v1, StageTag.T2v2, StageTag.T2v2 });
	}


	public static StageTag GetTagsFromCurrentPlayers(){
		InitializeTagList ();
		int activePlayers = PlayerStateController.GetNumberOfActivePlayers();
		int numberOfTeams = PlayerStateController.GetNumberOfTeams ();
		if (activePlayers < 2) {
			return StageTag.None;
		}
		int index = -1;
		int index2 = -1;
		if (!MasterStateController.GetTheMasterData ().teams) {
			index = 0;
			index2 = activePlayers;
		} else if (numberOfTeams > 2) {
			index = 0;
			index2 = numberOfTeams;
		} else {
			index = 1;
			index2 = activePlayers;
		}
		index2 -= 2; // offset
		return tagList [index] [index2];
	}

	public static bool CanContinueFromPlayerSelect(){
		if (PlayerStateController.GetNumberOfActivePlayers () >= 2) {
			if (MasterStateController.GetTheMasterData ().teams) {
				if (PlayerStateController.GetNumberOfTeams () >= 2) {
					if (MasterStateController.GetTheMasterData ().mode == GrandMode.TEAMFFA) {
						if (PlayerStateController.GetNumberOfPlayersInATeam() > 4) {
							return false;
						}
					}
					return true;
				}
			} else {
				return true;
			}
		}
		return false;
	}
}

