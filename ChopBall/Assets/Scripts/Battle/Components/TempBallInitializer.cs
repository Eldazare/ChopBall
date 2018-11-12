using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBallInitializer : MonoBehaviour {

    private Ball ball;

    private void Start()
    {
        ball = GetComponent<Ball>();
        ball.Initialize(null);
    }
}
