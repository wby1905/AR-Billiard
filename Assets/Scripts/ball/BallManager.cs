using System;
using System.Collections;
using Microsoft.MixedReality.Toolkit.UI;
using Unity.VisualScripting;
using UnityEngine;


public class BallManager : MonoBehaviour
{
    public Notification notification;
    private CueBall cueBall;
    public Stick stick;

    private Ball[] balls;
    private bool isAllStop;
    public bool isReplacing;

    public GameObject replace;
    private float _timer = 0f;
    private float _replaceTime = 5f;

    private Vector3 _prevPosition;


    void Start()
    {
        balls = GetComponentsInChildren<Ball>();
        cueBall = GetComponentInChildren<CueBall>();
        isAllStop = false;
        isReplacing = false;
    }

    void FixedUpdate()
    {
        if (_timer > 0f)
        {
            if (_prevPosition == cueBall.transform.position)
                _timer -= Time.fixedDeltaTime;
            _prevPosition = cueBall.transform.position;
            notification.SetText("Finish Replacing in " + _timer.ToString("0.0") + "s\n (Grab to continue replacement)");
        }
        else if (isReplacing)
        {
            FinishReplaceWhiteBall();
        }
    }

    public void checkIsAllStop()
    {
        foreach (Ball ball in balls)
        {
            if (ball.state == BallState.Moving)
            {
                isAllStop = false;
                return;
            }
        }
        isAllStop = true;
    }


    public bool IsAllStop()
    {
        return isAllStop && !isReplacing;
    }

    public int GetLastHit()
    {
        int res = cueBall.GetFirstHit();
        cueBall.ResetFirstHit();
        return res;
    }

    public int[] GetBallInHole()
    {
        ArrayList ballInHole = new ArrayList();
        foreach (Ball ball in balls)
        {
            if (ball.state == BallState.InHole)
            {
                ballInHole.Add(ball.ballNumber);
                ball.state = BallState.Removed;
            }
        }
        return (int[])ballInHole.ToArray(typeof(int));
    }

    public void ReplaceWhiteBall()
    {
        isReplacing = true;
        _prevPosition = cueBall.transform.position;
        replace.SetActive(true);
        cueBall.GetComponent<Ball>().Reset();
        cueBall.GetComponent<ObjectManipulator>().enabled = true;
        Pause();
        _timer = 10f; // if no manipulation
    }

    public void StartTimer()
    {
        _timer = _replaceTime;
    }

    public void FinishReplaceWhiteBall()
    {
        isReplacing = false;
        replace.SetActive(false);
        cueBall.GetComponent<ObjectManipulator>().enabled = false;
        Resume();
        stick.hasShot = false;
    }

    public void Pause()
    {
        cueBall.GetComponent<Ball>().OnPause();
        foreach (Ball ball in balls)
        {
            ball.OnPause();
        }
    }

    public void Resume()
    {
        cueBall.GetComponent<Ball>().OnResume();
        foreach (Ball ball in balls)
        {
            ball.OnResume();
        }
    }
}