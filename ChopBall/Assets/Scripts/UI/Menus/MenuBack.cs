using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBack : MonoBehaviour
{

    public void Back()
    {
        Previous.FindObjectOfType<Previous>().GetComponent<Previous>().PreviousPanel.SetActive(true);

        /*GameObject[] gameObjectArray = GameObject.FindGameObjectsWithTag("Menu");

        foreach (GameObject go in gameObjectArray)
        {
            go.SetActive(false);
        }*/
    }
}
