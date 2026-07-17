# ThirdPersonCamera

Third person camera demonstration made with Unity version 6000.0.58.f2

<img src="Assets/Video/ThirdPersonCameraDemonstration.GIF" alt="demo" width="600">

## Setup

Download zip file and extract contents into an empty Unity project.

## Use
This demonstration is intented to be used in Unity Playmode. Settings can be applied during
both editmode and playmode. Settings are to be adjusted in the Unity Inspector.

## Bugs
1) If the user allows limitless pitch, camera movement along the Y-axis will be inverted after having been transformed enough
2) Polling rate of over 1000hz on the end user's mousing device may cause Unity to incorrectly register mouse movements
3) When pausing the demonstration with ESC the game pauses. When the users presses ESC to unpause the game, the game unpauses but the menu is not deactivated. Pressing resume when the game is running will cause the game to be paused and menu to be deactivated. This issue can be circumvented by pressing ESC again if the game is not paused in the menu and clicking resume 
