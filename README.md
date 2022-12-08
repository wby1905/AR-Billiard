# AR-Billiard

Created with MRTK 2.8 and 2021.3.10f1

# Features:
1. Physics system.
   The ball's movement is simulated by the Unity's phyX engine and the collision between Stick and Cue ball is manually calculated by the script.
2. Input (Gesture Detection)
   The input is designed to be compatible with the HoloLens 2. The user can use right hand to control the stick. And user may pinch to begin detailed adjustment (like the concrete side of the hit point) and hitting.
3. UI
   The UI is mainly inherited from the MRTK's default UI. The UI is designed to be simple and intuitive. The user can get the Score and the remaining balls by the UI. There are also some buttons to adjust table and reset game.
   * Aim point
        The aim points on the cue ball and the left UI are manually calculated with some vectors and without the help of Physics.Raycast to improve efficiency.
4. Sound
    The sound is designed to be simple and intuitive. The sound is played when the ball is hit and when the ball is pocketed. And the volumn of the sound is adjusted according to the speed of the ball.

All resources are obtaied via internet with the purpose of learning and practicing.
