using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    public TMP_Text _text;
    private float _timer = 0f;
    public float _fadeTime = 4f;



    public void FixedUpdate()
    {
        if (_timer <= 0f)
        {
            _text.text = "";
            gameObject.SetActive(false);
        }
        else
        {
            _timer -= Time.fixedDeltaTime;
        }
    }

    public void SetText(string text)
    {
        _text.text = text;
        _timer = _fadeTime;
        gameObject.SetActive(true);
    }
}
