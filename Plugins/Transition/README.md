type: YAML
owner: FredericRP
standardAsset: Yes
name: Transition
majorversion: 1
minorversion: 2
lastModification: 2019-08-01
unityVersionMinimum: Unity 2019

## Description

A simple transition that allows you to hide your game screen while you do stuff (load level, scene, or whatever).
There are two types of transition:
- Doors : use two images that hide the screen by scrolling from the left and right borders of the screen
- CutOff : use a gradient image to show/hide a texture fullscreen

## Usage

1. Place chosen prefab in scene
2. Subscribe to the TransitionHidden or TransitionShown game events to trigger at the right time what you want to do.
