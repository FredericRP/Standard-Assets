# Transition

Between scenes, to hide loading time, you have to show a transition screen. Those two prefabs allow you to call the transition to be shown or hidden.
Subscribe to its events to know when you can load something behind the players back.
Requires the EventHandler and Singleton.

![demo](../../images/transition_demo.gif)

![how it is supposed to work](../../images/transition_schema.jpg)

## Description

There are two types of transition:
- Doors : use two images that hide the screen by scrolling from the left and right borders of the screen
- CutOff : use a gradient image to show/hide a texture fullscreen

## Usage

1. Place chosen prefab in scene
2. Call Transition.Show()
3. Do whatever you want behind the player's back
4. Call Transition.Hide()

Optionnal : subscribe to the game events to trigger at the right time what you want to do.
