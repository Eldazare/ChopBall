using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVTest : MonoBehaviour {

    public GameObject Screen;
    private IEnumerator coroutine;

    void Start()
    {
        coroutine = Wait(15.0f);
        StartCoroutine(coroutine);

        Screen.SetActive(false);
    }

    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Screen.SetActive(true);
            yield return (new WaitForSeconds(5));
            Screen.SetActive(false);
        }
    }
}
