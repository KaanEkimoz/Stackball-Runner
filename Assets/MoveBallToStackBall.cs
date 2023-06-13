using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBallToStackBall : MonoBehaviour
{
    private Transform stackBallPos;
    private bool isStart;
    private GameObject ball;
    private void Start()
    {
        stackBallPos = this.gameObject.transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        isStart = true;
        ball = other.gameObject;
    }

    private void Update()
    {
        if (ball != null)
        {
            if (isStart)
            {
                ball.transform.position = new Vector3(Mathf.Lerp(ball.transform.position.x, stackBallPos.position.x, 2* Time.deltaTime),
                    Mathf.Lerp(ball.transform.position.y, stackBallPos.position.y, 2* Time.deltaTime), 
                    Mathf.Lerp(ball.transform.position.z, stackBallPos.position.z, 2* Time.deltaTime));
            }

            if (ball.transform.position.z > stackBallPos?.transform.position.z -0.2f && isStart)
            {
                isStart = false;
                ball.GetComponent<Player>().playerState = Player.PlayerState.Playing;
            }
        }
        
    }
}
