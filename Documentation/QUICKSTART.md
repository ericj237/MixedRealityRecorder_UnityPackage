# Quick Start Guide: Mixed Reality Recorder for Unity

This guide will get you ready to record users of Virtual Reality applications inside the digital environments.

## 1. Hardware Setup

1.1 [Setup a studio with greenscreen background.](https://www.wikihow.com/Set-Up-a-Green-Screening-Studio)
1.2 Mount the Vive tracker on the camera.

BILD!

1.3 Connect the camera by usb or with a HDMI capture card to the PC.

## 2. Software Setup

2.1 [Import the package in your Unity project.](https://docs.unity3d.com/Manual/AssetPackagesImport.html)

<img src="./Ressources/Images/img_importPackage01.png" width="50%">

2.2 Include the prefab `MixedRealityRecorder` in your scene.

<img src="./Ressources/Images/img_includePrefab.png" width="50%">

2.3 Tag the HMD object with "Target".

<img src="./Ressources/Images/img_tagTarget.png" width="50%">

2.4 Start your application.
2.5 Select the physical camera source.
2.6 Select the foreground target.
2.6 Match the camera settings in the user interface.
2.7 Select the output format and optional path.
2.8 Apply the sensor offset ranging from the center of the Vive tracker to the center of the camera sensor in cm.

BILD!

2.9 Record your application.

## Tested HDMI Capture Cards

MRR was tested with the listed HDMI Capture Cards.

| Tested HDMI Capture Cards                                                   |
| --------------------------------------------------------------------------- |
| [Blackmagic ATEM mini](https://www.blackmagicdesign.com/products/atemmini)  |

All HDMI capture cards which are detected as webcam devices by Unity can be used with MRR.
