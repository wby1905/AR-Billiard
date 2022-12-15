using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;
using UnityEngine;

public class ToggleHand : MonoBehaviour
{
    private bool _isClicked;

    public SolverHandler startPoint;


    // Start is called before the first frame update
    void Start()
    {
        _isClicked = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Toggle()
    {
        _isClicked = !_isClicked;
        if (_isClicked)
        {
            startPoint.TrackedTargetType = TrackedObjectType.HandJoint;
            startPoint.TrackedHandedness = Handedness.Left;
            startPoint.TrackedHandJoint = TrackedHandJoint.ThumbProximalJoint;

        }
        else
        {
            startPoint.TrackedTargetType = TrackedObjectType.CustomOverride;
        }
    }
}
