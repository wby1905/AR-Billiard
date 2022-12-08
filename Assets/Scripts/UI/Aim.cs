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

    private float _tipTimer = 0f;
    private float _tipFadeTime = 1f;

    private string _DEFAULT_TIP = "Pinch to Aim";
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        aimPoint.position = cueBallAim.transform.position - cueBall.transform.position + stick.aimPoint.position;
        aimPoint.up = stick.transform.up;
        degree.text = (90 - Vector3.Angle(stick.transform.up, Vector3.up)).ToString("0.0") + "°";
    }

    void FixedUpdate()
    {
        if (_tipTimer <= 0f)
        {
            tip.text = _DEFAULT_TIP;
        }
        else
        {
            _tipTimer -= Time.fixedDeltaTime;
        }

    }

    public void SetTip(string text)
    {
        tip.text = text;
        _tipTimer = _tipFadeTime;
    }
}
