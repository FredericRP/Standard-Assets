using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FredericRP.EventManagement
{
  public class EventHandler
  {
    private static Dictionary<GameEvent, Delegate> gameEvents = new Dictionary<GameEvent, Delegate>();

    public static void AddEventListener(GameEvent gameEvent, UnityAction value)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] + value;
    }

    public static void AddEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      if (!gameEvents.ContainsKey(gameEvent))
      {
        gameEvents.Add(gameEvent, null);
      }

      gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] + handler;
    }

    public static void RemoveEventListener(GameEvent gameEvent, UnityAction handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction)gameEvents[gameEvent] - handler;
      }
    }

    public static void RemoveEventListener<T>(GameEvent gameEvent, UnityAction<T> handler)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        gameEvents[gameEvent] = (UnityAction<T>)gameEvents[gameEvent] - handler;
      }
    }

    public static bool TriggerEvent(GameEvent gameEvent)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction callback = eventDelegate as UnityAction;

        if (callback != null)
        {
          callback();

          return true;
        }
      }

      return false;
    }

    public static bool TriggerEvent<T>(GameEvent gameEvent, T value)
    {
      Delegate eventDelegate;

      if (gameEvents.TryGetValue(gameEvent, out eventDelegate))
      {
        UnityAction<T> callback = eventDelegate as UnityAction<T>;

        if (callback != null)
        {
          callback(value);
          return true;
        }
      }

      return false;
    }
  }
}