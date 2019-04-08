using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGoalScoreDisplayer : MonoBehaviour
{
	public List<GameObject> allTextObjectsList;
	private List<GameObject> displayObjectList;
	private List<Text> displayTexts;


	public List<Text> goalStockDisplays;
	private PlayerStateData[] states;
	private List<string> characterNames;
	private List<CharacterAttributeData> listedAttributes;

    public void ResolutionChangeUpdatePositions()
    {
		states = PlayerStateController.GetAllStates ();
		listedAttributes = new List<CharacterAttributeData> ();
		characterNames = new List<string> ();
		foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors()) {
			characterNames.Add(CharacterAttributeController.GetACharacter(
				states[competitor.playerID-1].characterChoice).CharacterName
			);
			listedAttributes.Add (CharacterAttributeController.GetACharacter (states [competitor.playerID-1].characterChoice));
		}

        if (displayObjectList != null)
        {
            if (displayObjectList.Count == 0)
            {
                return;
            }
            int height = 1080;
            int width = 1920;
            Debug.Log("Height :" + Screen.height + " Width: " + Screen.width);

            float yPos = (float)height * -0.5f + 75f;
            float relativeX = (float)width / displayObjectList.Count;
            float xPos = (float)width * -0.5f + relativeX / 2.0f;
            for (int i = 0; i < displayObjectList.Count; i++)
            {
                Debug.Log(xPos + "   " + yPos);
                displayObjectList[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
                xPos += relativeX;
            }
        }
    }

    public void StatsUpdated()
    {
        if (CurrentBattleController.GetTeams() != null)
        {
            if (displayObjectList == null)
            {
				displayObjectList = new List<GameObject>();
				displayTexts = new List<Text> ();
                PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
                int j = 0;
                foreach (TeamContainer team in CurrentBattleController.GetTeams())
                {
                    displayObjectList.Add(allTextObjectsList[j]);
                    displayObjectList[j].gameObject.SetActive(true);
					displayTexts.Add (displayObjectList [j].GetComponentInChildren<Text> ());
					displayTexts[j].color = pBaseData.teamColors[team.teamID];
                    j++;
                }
                ResolutionChangeUpdatePositions();
            }

            int i = 0;
			foreach (TeamContainer team in CurrentBattleController.GetTeams())
			{
//				displayTexts[i].text = "Team " + team.teamID;
//				displayTexts[i].text += "\nScore: " + team.score;
//				i++;
			}

        }
        else
        {
            if (displayObjectList == null)
            {
				displayObjectList = new List<GameObject>();
				displayTexts = new List<Text> ();
                PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
                int j = 0;
                foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors())
                {
                    displayObjectList.Add(allTextObjectsList[j]);
                    displayObjectList[j].gameObject.SetActive(true);
					displayTexts.Add (displayObjectList [j].GetComponentInChildren<Text> ());
					displayTexts[j].color = pBaseData.playerColors[competitor.playerID - 1];
                    j++;
                }
                ResolutionChangeUpdatePositions();
            }

            int i = 0;
            foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors())
            {
//				displayTexts[i].text = characterNames[i];
//                displayTexts[i].text += "\nScore: " + competitor.score;
//                i++;
            }
        }
        if (CurrentBattleController.IsStockActive() && goalStockDisplays.Count > 0)
        {
            for (int i = 0; i < CurrentBattleController.GetGoalInfos().Count; i++)
            {
				goalStockDisplays [i].text = CurrentBattleController.GetGoalInfos () [i].GetStockString ();
            }
        }
        else
        {
            foreach (var text in goalStockDisplays)
            {
                text.text = "";
            }
        }
    }
}