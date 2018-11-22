using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameScoreboard : MonoBehaviour
{

    public List<Text> allTextList;
    public Text timerDisplay;
    //public List<Text> goalStockDisplays;

    private List<Text> displayTextList;

    public void TimerUpdated()
    {
        timerDisplay.text = CurrentBattleController.GetATime();
    }

    public void StatsUpdated()
    {
        if (CurrentBattleController.GetTeams() != null)
        {
            if (displayTextList == null)
            {
                displayTextList = new List<Text>();
                PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
                int j = 0;
                foreach (TeamContainer team in CurrentBattleController.GetTeams())
                {
                    displayTextList.Add(allTextList[j]);
                    displayTextList[j].gameObject.SetActive(true);
                    displayTextList[j].color = pBaseData.teamColors[team.teamID];
                    j++;
                }
            }

            int i = 0;
            foreach (TeamContainer team in CurrentBattleController.GetTeams())
            {
                displayTextList[i].text = "Team " + team.teamID;
                displayTextList[i].text += "\nGoals: " + team.goals;
                //displayTextList[i].text += "\nScore: " + team.score;
                i++;
            }

        }
        else
        {
            if (displayTextList == null)
            {
                displayTextList = new List<Text>();
                PlayerBaseData pBaseData = (PlayerBaseData)Resources.Load("Scriptables/_BaseDatas/PlayerBaseData", typeof(PlayerBaseData));
                int j = 0;
                foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors())
                {
                    displayTextList.Add(allTextList[j]);
                    displayTextList[j].gameObject.SetActive(true);
                    displayTextList[j].color = pBaseData.playerColors[competitor.playerID - 1];
                    j++;
                }
            }

            int i = 0;
            foreach (CompetitorContainer competitor in CurrentBattleController.GetCompetitors())
            {
                displayTextList[i].text = "Player " + competitor.playerID;
                displayTextList[i].text += "\nGoals: " + competitor.goalsScored;
                //displayTextList[i].text += "\nScore: " + competitor.score;
                i++;
            }
        }
        /*if (CurrentBattleController.IsStockActive() && goalStockDisplays.Count > 0)
        {
            for (int i = 0; i < CurrentBattleController.GetGoalInfos().Count; i++)
            {
                goalStockDisplays[i].text = CurrentBattleController.GetGoalInfos()[i].stocks.ToString();
            }
        }
        else
        {
            foreach (var text in goalStockDisplays)
            {
                text.text = "";
            }
        }*/
    }
}