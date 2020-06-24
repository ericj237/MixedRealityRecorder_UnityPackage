# Quick Start Guide: Mixed Reality Recorder for Unity

This guide will get you ready to record users of Virtual Reality applications inside the digital environments.

## 1. Hardware Setup

1.1 [Setup a studio with greenscreen background.](https://www.wikihow.com/Set-Up-a-Green-Screening-Studio)<br/>
1.2 Mount the Vive tracker on the camera.

<img src="./Ressources/Images/img_mountTracker01.jpg" width="35%">

1.3 Connect the camera by usb or with a HDMI capture card to the PC.

## 2. Software Setup

2.1 [Import the package in your Unity project.](https://docs.unity3d.com/Manual/AssetPackagesImport.html)

<img src="./Ressources/Images/img_importPackage01.png" width="50%">

2.2 Include the prefab `MixedRealityRecorder` as a child object under your camera rig / player.
2.3 Tag the HMD object with "Target".

<img src="./Ressources/Images/img_tagTarget.png" width="50%">

2.4 Tag the camera rig / player with "Scene".

## 3 Production

3.1 Start your application.<br/>
3.2 Select the physical camera source.<br/>
3.3 Select the foreground target.<br/>
3.4 Match the camera settings in the user interface.<br/>
3.5 Select the output format and optional path.<br/>
3.6 Apply the sensor offset ranging from the center of the Vive tracker to the center of the camera sensor in cm.<br/>

<img src="./Ressources/Images/img_measureTracker01.jpg" width="35%">

2.9 Record your application.<br/>

## Tested HDMI Capture Cards

MRR was tested with the listed HDMI Capture Cards.

| Tested HDMI Capture Cards                                                   |
| --------------------------------------------------------------------------- |
| [Blackmagic ATEM mini](https://www.blackmagicdesign.com/products/atemmini)  |

All HDMI capture cards which are detected as webcam devices by Unity can be used with MRR.
