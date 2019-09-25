# Standard-Assets
Assets used in almost all of our projects. Some are quite old but still useful.

## Object Pool & Inspector

An object pool is important and often required in video games project. Shown and explained in my YouTube (french) video here, you can find this (old) sources right there.

![object pool inspector](images/object_pool.png)

## Transition

Between scenes, to hide loading time, you have to show a transition screen. This two prefabs allow you to call the transition to be shown or hidden and subscribe to its event to know when you can load something behind the players back.
Requires the EventHandler and Singleton.

![demo](images/transition_demo.gif)

![how it is supposed to work](images/transition_schema.jpg)

## Singleton

A generic singleton class that allows you to have Singleton in your project without reimplementing it every time and YES, I use Singleton !

## Event Handler

A generic event handler that allows you to subscribe and trigger game events in your game in a minute. GameEvents have evolved thanks to the Unite 2017 presentation by Schell Games (but we still prefer our way to handle listener with delegates).