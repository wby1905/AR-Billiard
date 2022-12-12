using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Aim : MonoBehaviour
{

    public Stick stick;
    public CueBall cueBall;

    public TMP_Text tip;
    public TMP_Text degree;
    public GameObject cueBallAim;

    public Transform aimPoint;
    public float radius = 0.5f;


    private string _DEFAULT_TIP = "Pinch to Aim";

    // Update is called once per frame
    void Update()
    {
        aimPoint.position = cueBallAim.transform.position - (cueBall.transform.position - stick.aimPoint.position) * radius / (stick.radius * stick.scale);
        if (Vector3.Distance(aimPoint.position, cueBallAim.transform.position) > radius + 0.05f)
        {
            aimPoint.position = Vector3.zero;
        }
        aimPoint.up = -stick.transform.forward;
        degree.text = (-90 + Vector3.Angle(-stick.transform.forward, Vector3.up)).ToString("0.0") + "°";
    }

    public void ResetTip()
    {
        tip.text = _DEFAULT_TIP;
    }

    public void SetTip(string text)
    {
        tip.text = text;
    }
}
