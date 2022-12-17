This project follows MIT LICENSE.

# AR-Billiard

Created with MRTK 2.8 and 2021.3.10f1

Demo: 
https://www.youtube.com/watch?v=-ZDgUf9-dPM
+ Adjust:
![Adjust](imgs/Adjust.gif)
+ Shoot with Single Hand:
![Single](imgs/Single.gif)
+ Shoot with Two Hands:
![Both](imgs/Both.gif)
+ Adjust Cue Ball:
![CueBall](imgs/CueBall.gif)

# How to run:
After Download the whole project, you can open Unity editor with the specific version. If you want to run it in the editor, you need to add two scenes in `/scene` folder, which is `ARPoolGame` scene and `Manager` scene. Then you can hit the play button to run it.

To Deploy, follow [the MRTK website](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/?view=mrtkunity-2022-05) to deploy on HoloLens 2. (Basically you need Visual Studio to deploy).


# Features:
1. Physics system.
   The ball's movement is simulated by the Unity's phyX engine and the collision between Stick and Cue ball is manually calculated by the script.
   Core script in `Stick.cs`:
```csharp
    void OnTriggerEnter(Collider other)
    {
        if (Vector3.Dot(transform.forward, _speed) > 0f) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("CueBall") &&
        _timer <= 0f && _speed.magnitude > 0.02f && _isLocked && !HasShot())
        {

            // ToDO Currently there is no torque Don't know why AddForceAtPosition is not working
            other.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(_speed / Time.fixedDeltaTime * _forceFactor, other.ClosestPoint(transform.position), ForceMode.Force);
            _audioSource.volume = _speed.magnitude * 0.3f;
            _audioSource.Play();
            deactivate();
            _timer = _fadeTime;
            hasShot = true;
        }
    }
```
   
   
   
2. Input (Gesture Detection)
   The input is designed to be compatible with the Ho loLens 2. The user can use right hand to control the stick. And user may pinch to begin detailed adjustment (like the concrete side of the hit point) and hitting.
Core script in `Stick.cs`:
```csharp
   // Gesture Detection
    public void OnActionStarted(BaseInputEventData eventData)
    {
        if (eventData.InputSource.SourceName.Contains("Right"))
        {
            Lock();
            settings.SetActive(false);
        }
    }

    public void OnActionEnded(BaseInputEventData eventData)
    {
        if (eventData.InputSource.SourceName.Contains("Right"))
        {
            Unlock();
            settings.SetActive(true);
        }

    }
    
    // And a lot of vector calculations in Update...
    void Update()
    {
        if (start.GetComponent<SolverHandler>().TrackedTargetType == TrackedObjectType.CustomOverride)
        {
            endDistance /= 2f;
        }
        if ((canInstantiate() || (_isLocked && _line.enabled == true)) && _timer <= 0f && !hasShot)
        {
            activate();
            float remap = 0f;
            scale = (transform.lossyScale.x / _initScale.x);
            float curRadius = radius * scale;
            if (!_isLocked)
            {
                Ray ray = new Ray(end.position, start.position - end.position);
                transform.position = ray.GetPoint(endDistance * scale);
                transform.forward = -(start.position - end.position);

            }

            else
            {
                Vector3 reproject = Vector3.Project(start.position - end.position, -transform.forward);
                Vector3 vertical = reproject - (start.position - end.position);
                // This is the remapped distance on the vertical plane of the stick.
                remap = Mathf.Clamp(vertical.magnitude, 0f, curRadius * 10f) / 10f;
                transform.position = start.position - reproject;
                // To make it easier to shoot, we may also want to add an offset to current position.

                // single hand
                if (start.GetComponent<SolverHandler>().TrackedTargetType == TrackedObjectType.CustomOverride)
                {
                    transform.position += (vertical.normalized * remap + reproject.normalized * endDistance * scale);
                }

            }

            if (Vector3.Dot(startPoint.position - aim.cueBall.transform.position, transform.forward) < 0f && _speed.magnitude < 2f)
            {
                Ray ray = new Ray(transform.position, -transform.forward);
                transform.position = ray.GetPoint(-Vector3.Distance(startPoint.position, aim.cueBall.transform.position) - 0.05f * scale);
                _isShortened = true;
            }
            else
            {
                if (_isShortened)
                {
                    _speed = Vector3.zero;
                    prevPosition = Vector3.zero;
                }
                _isShortened = false;
            }

            Ray rayToBall = new Ray(transform.position, -transform.forward);
            float dis = Mathf.Sqrt(Mathf.Pow(curRadius, 2) - Mathf.Pow(remap, 2));
            float dis2 = Mathf.Sqrt(Mathf.Pow(Vector3.Distance(transform.position, start.position), 2) - Mathf.Pow(remap, 2));
            aimPoint.position = rayToBall.GetPoint((dis2 - dis - 0.005f * scale));
            aimPoint.up = -transform.forward;

            drawAimLine();
        }
        else
        {
            deactivate();
        }

    }
```
   
   
3. UI
   The UI is mainly inherited from the MRTK's default UI. The UI is designed to be simple and intuitive. The user can get the Score and the remaining balls by the UI. There are also some buttons to adjust table and reset game.
   * Aim point
        The aim points on the cue ball and the left UI are manually calculated with some vectors and without the help of Physics.Raycast to improve efficiency.
4. Sound
    The sound is designed to be simple and intuitive. The sound is played when the ball is hit and when the ball is pocketed. And the volumn of the sound is adjusted according to the speed of the ball.
5. Rule
   The rule is following the American Pooling rule set. Several status are defined as a feedback after each hit.
```csharp
public enum RuleResult
{
    Error,
    WhiteBall,
    Foul,
    ChangePlayer,
    ContinueHit,
    Win,
    Lose
}
```
Also the `Rule` is generic and feel free to add other Ruel sets!

    
# Acknowledgment
All resources are obtaied via internet with the purpose of learning and practicing.
