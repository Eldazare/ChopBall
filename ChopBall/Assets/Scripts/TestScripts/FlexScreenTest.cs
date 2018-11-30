using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexScreenTest : MonoBehaviour
{

    public GameObject Screen;
    //public GameObject ScreenCamera;

    private IEnumerator coroutine;

    void OnEnable()
    {
        Screen.SetActive(true);
        //ScreenCamera.SetActive(true);
        coroutine = Wait(3.0f);
        StartCoroutine(coroutine);
    }

    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Screen.SetActive(false);
            //ScreenCamera.SetActive(false);
        }
    }
}