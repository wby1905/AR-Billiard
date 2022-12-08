using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;

public class Adjust : MonoBehaviour
{
    public GameObject billiards;
    public Stick stick;
    private bool _isClicked = false;

    public void OnClick()
    {
        _isClicked = !_isClicked;
        if (_isClicked)
        {
            billiards.GetComponent<ObjectManipulator>().enabled = true;
            stick.deactivate();
            stick.gameObject.SetActive(false);
        }
        else
        {
            billiards.GetComponent<ObjectManipulator>().enabled = false;
            stick.activate();
            stick.gameObject.SetActive(true);
        }
    }

}
