# Event Management

A generic event handler that allows you to subscribe and trigger game events in your game in a minute.

GameEvents have evolved thanks to the Unite 2017 presentation by Schell Games (but we still prefer our way to handle listener with delegates).

## Usage

1. Create new game events from the project view using Create / FredericRP / [...] Game Event (with a typed parameter or no parameter)

2. Use it by Code
- add a delegate that will receive the event using AddEventListener (and RemoveEventListener)
- trigger the event using TriggerEvent

3. **OR** Use it with provided *SimpleEventTrigger*
- put this MonoBehaviour on a GameObject
- assign the game event you just created
- add as many unity events as you want to call functions

See the **[Screen Transition](../ScreenTransitions) demo**.