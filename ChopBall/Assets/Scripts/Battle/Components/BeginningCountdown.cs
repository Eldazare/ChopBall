using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginningCountdown : MonoBehaviour {

    public List<Sprite> spriteList; // 0 is CHOP, 1-3 are numbers.
    public Image image;


    public delegate void CountDownReturn();

    public void CountDown(CountDownReturn del){
        if (image != null) { 
            StartCoroutine(CDIenum(del));
        } else {
            del();
        }
    }

    private IEnumerator CDIenum(CountDownReturn del) {
        image.enabled=true;
        for (int i = 3; i > 0; i--) {
            image.sprite = spriteList[i];
            yield return new WaitForSeconds(1f);
        }
        image.sprite = spriteList[0];
        yield return new WaitForSeconds(0.4f);
        image.enabled=false;
        del();
    }
}
